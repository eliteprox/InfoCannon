package drivers

import (
	"crypto"
	"crypto/rand"
	"crypto/rsa"
	"crypto/sha256"
	"crypto/x509"
	"encoding/base64"
	"encoding/json"
	"encoding/pem"
	"errors"
	"fmt"
	"io/ioutil"
	"time"

	"github.com/livepeer/go-livepeer/net"
)

type (
	gsKeyJSON struct {
		Type                    string `json:"type,omitempty"`
		ProjectID               string `json:"project_id,omitempty"`
		PrivateKeyID            string `json:"private_key_id,omitempty"`
		PrivateKey              string `json:"private_key,omitempty"`
		ClientEmail             string `json:"client_email,omitempty"`
		ClientID                string `json:"client_id,omitempty"`
		AuthURI                 string `json:"auth_uri,omitempty"`
		TokenURI                string `json:"token_uri,omitempty"`
		AuthProviderX509CertURL string `json:"auth_provider_x509_cert_url,omitempty"`
		ClientX509CertURL       string `json:"client_x509_cert_url,omitempty"`
	}

	gsSigner struct {
		jsKey     *gsKeyJSON
		parsedKey *rsa.PrivateKey
	}

	gsOS struct {
		s3OS
		gsSigner *gsSigner
	}

	gsSession struct {
		s3Session
	}
)

func gsHost(bucket string) string {
	return fmt.Sprintf("https://%s.storage.googleapis.com", bucket)
}

func gsParseKey(key []byte) (*rsa.PrivateKey, error) {
	if block, _ := pem.Decode(key); block != nil {
		key = block.Bytes
	}
	parsedKey, err := x509.ParsePKCS8PrivateKey(key)
	if err != nil {
		parsedKey, err = x509.ParsePKCS1PrivateKey(key)
		if err != nil {
			return nil, err
		}
	}
	parsed, ok := parsedKey.(*rsa.PrivateKey)
	if !ok {
		return nil, errors.New("oauth2: private key is invalid")
	}
	return parsed, nil
}

func NewGoogleDriver(bucket, keyFileName string) (OSDriver, error) {
	os := &gsOS{
		s3OS: s3OS{
			host:   gsHost(bucket),
			bucket: bucket,
		},
	}
	rawFile, err := ioutil.ReadFile(keyFileName)
	if err != nil {
		return nil, err
	}
	var gsKey gsKeyJSON
	if err := json.Unmarshal(rawFile, &gsKey); err != nil {
		return nil, err
	}
	parsedKey, err := gsParseKey([]byte(gsKey.PrivateKey))
	if err != nil {
		return nil, err
	}
	os.gsSigner = &gsSigner{
		jsKey:     &gsKey,
		parsedKey: parsedKey,
	}
	return os, nil
}

func (os *gsOS) NewSession(path string) OSSession {
	var policy, signature = gsCreatePolicy(os.gsSigner, os.bucket, os.region, path)

	sess := os.s3OS.NewSession(path).(*s3Session)
	sess.credential = os.gsSigner.clientEmail()
	sess.host = gsHost(os.bucket)
	sess.policy = policy
	sess.signature = signature
	return &gsSession{s3Session: *sess}
}

func (os *gsSession) SaveData(name string, data []byte) (string, error) {
	fields := map[string]string{
		"GoogleAccessId": os.credential,
		"signature":      os.signature,
	}
	return os.saveData(name, data, fields)
}

func (os *gsSession) GetInfo() *net.OSInfo {
	oi := os.s3Session.GetInfo()
	oi.StorageType = net.OSInfo_GOOGLE
	return oi
}

func newGSSession(info *net.S3OSInfo) OSSession {
	sess := newS3Session(info).(*s3Session)
	return &gsSession{s3Session: *sess}
}

// gsCreatePolicy returns policy, signature
func gsCreatePolicy(signer *gsSigner, bucket, region, path string) (string, string) {
	const timeFormat = "2006-01-02T15:04:05.999Z"
	const shortTimeFormat = "20060102"

	expireAt := time.Now().Add(S3_POLICY_EXPIRE_IN_HOURS * time.Hour)
	expireFmt := expireAt.UTC().Format(timeFormat)
	src := fmt.Sprintf(`{ "expiration": "%s",
    "conditions": [
      {"bucket": "%s"},
      {"acl": "public-read"},
      ["starts-with", "$Content-Type", ""],
      ["starts-with", "$key", "%s"]
    ]
  }`, expireFmt, bucket, path)
	policy := base64.StdEncoding.EncodeToString([]byte(src))
	sign := signer.sign(policy)
	return policy, sign
}

func (s *gsSigner) sign(mes string) string {
	h := sha256.New()
	h.Write([]byte(mes))
	d := h.Sum(nil)

	signature, err := rsa.SignPKCS1v15(rand.Reader, s.parsedKey, crypto.SHA256, d)
	if err != nil {
		panic(err)
	}

	return base64.StdEncoding.EncodeToString(signature)
}

func (s *gsSigner) clientEmail() string {
	return s.jsKey.ClientEmail
}
