# InfoCannon

A tool for automating the upload of Infowars videos to Facebook

**Tool Name**: InfoCannon  
**Source Code**: https://github.com/PatriotDevOps/InfoCannon  
**Language**: C# .Net 4.5 WinForms  
**Use Case**: Mass upload Infowars show archives and Special Reports into Facebook daily. For use on personal newsfeed or public page.  RIP [Infowars Live Feed](https://facebook.com/infowarslivefeed)  

**Configuration Directions**:  
1. Go to [developers.facebook.com](https://developers.facebook.com) and Create a Facebook App. You can name it something like "video uploader"
2. After you create your app you should be on the App Dashboard page. If you need help finding it, go here and click on your app: https://developers.facebook.com/apps/
3. From the App Dashboard, be sure that your app is not "Live". This is controlled by a switch in the top right corner of the page. If it is, turn it off.
4. Navigate to **Tools** > **Graph API Explorer**. Open it in a new tab.
5. From Graph API Explorer, click **Get Token**, then choose **Get User Access Token**.
6. From the **Select Permissions** pop-up, choose the following:
   * publish_pages
   * manage_pages
   * pages_show_list
7. Click **Get Access Token**.
8. You may use this token for publishing to your own newsfeed, or to obtain a page access token, follow these additional steps:
   1. Click **Get Token** again, 
   2. Under the **Page Access Tokens** section, choose the name of the page you want to publish to. 
   3. Finally, click **Get Token** again, and choose **Request_Pages**. A dialog prompt appears, click **Continue As (Your Name)**
   4. You will be prompted which pages you want to grant access to, leave all checked or change the selections, click **Next**.
   5. Leave both **Manage your Pages** and **Publish as Pages you manage** set to **Yes**.
   6. Click **Done**. Click **OK**.
9. Copy the Access Token code generated in Graph API Explorer
10. Open **InfoCannon** and paste the access token into the **Access Key** textbox.
11. From InfoCannon, set the **Page ID** accordingly:
   * If using the User Access Token to post to your own page, enter  **me**.
   * If using a Page Access Token to post to a page you manage, obtain the **Page ID** from the **About** section of your page. This is found at the very bottom of your page's About section.
12. From **InfoCannon**, click the **Test** button to fire off a post that simply says "This is a test". If this is successful, proceed to the next step. Keep in mind nobody can see the post at this stage except for you. However, if you do not delete the  post it will become visible once the app goes live.
13. Click **Save** to remember the settings, they will re-appear when you open the tool. They are kept in a text file in the tool's current directory.

**Considerations**
1. You will need to repeat the above steps 4-12 each time that you make the app "live". This is a workaround to a restriction Facebook has placed on the developer API. When the app is in "Live" mode you cannot obtain a valid API key with public permissions. However, you can do so when the app is in Developer mode, and use it. Upon placing the app in "Live" status, your videos will become public and your developer Access Token will be made invalid. Each time you upload videos, you will need to place the app in "Developer Mode" before generating a new Access Token key with Graph API Explorer. Make sense? I hope so.

**Using the Tool**
1. Select the show: Alex Jones Show, David Knight, War Room, or Special Reports
2. Select the date to gather videos from (The tool caches only the last 100 or so, don't go back more than a month or two.)
3. Click **Gather Videos**, if you have never uploaded this video before then it will have a Checkmark in the box to the left, otherwise it is unchecked.
4. Click **Post Videos**, all videos in the above listbox that are checked will be uploaded and posted to the page with full descriptions.

**NOTES**:
1. DO NOT submit your app for Review, under any circumstances
2. Use backup accounts if possible. Best not to do with an account for which you don't have data backed up on.
3. If you ever want to remove ALL the videos uploaded, you can do so by deleting the app from developers.facebok.com. Conversely, do not delete the app unless you intend to lose all videos uploaded by InfoCannon. This can be useful if you need to "go dark" quickly.
