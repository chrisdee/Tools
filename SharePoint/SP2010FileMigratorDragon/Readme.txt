It is quite common that, during SharePoint implementations, end users want to migrate file and folder structures located on the file system to a SharePoint document library. Out of the box, you can do this by uploading multiple documents or via the Windows Explorer view. Both ways are quite easy to use but have several disadvantages:

When migrating larger amounts of data both methods are not really reliable. 
Failure usually means starting all over again.
Both methods don’t offer adequate insight on current progress.
Another approach would be to use third party migration tools. There are very good ones out there, but they have disadvantages too:

Obviously, they cost money.
They’re usually less easy to use (compared to the previous method).
They’re typically used by administrators, which leaves end users wanting to 
migrate files on the file system themselves in the cold.
Another approach would be to use the Microsoft SyncToy (http://sharepointdragons.com/2012/04/10/synctoy/) tool. SyncToy is a tool that synchronizes files and folders between locations using the Microsoft Sync Framework 2.0. Luckily, by mapping a network drive pointing to a SharePoint document library (http://grounding.co.za/blogs/neil/archive/2008/08/02/using-synctoy-to-synchronize-offline-sharepoint-documents-on-vista.aspx), you can use SyncToy to migrate files from the file system to SharePoint 2010. When doing this, SyncToy leverages the SharePoint Sync Framework API (http://msdn.microsoft.com/en-us/library/ee538641.aspx) to sync the file system to the SharePoint list. Or, to put it in other words, SharePoint 2010 has its own sync provider which can be consumed by clients (such as SyncToy and most notably, SharePoint Workspace 2010): http://www.chakkaradeep.com/post/SharePoint-2010-ListsGetListItemChangesWithKnowledge-Method.aspx.

The SyncToy has disadvantages too: 

It synchronizes files and folders (two-way copy), that’s something quite different from migrating files and folders (one-way copy). For larger structures, this adds considerable overhead and complexity. You should avoid synchronization, unless that’s really what you’re after.
It requires a network drive mapping. Most end users won’t know how to do this, and usually in larger organizations end users won’t be allowed to create them anyway. For such organizations it simply won’t be feasible to let admins create network drive mappings whenever end users need to migrate something to a SharePoint library.
It’s easy for end users to make a mistake. For example, renaming or deleting files in the SharePoint library may have severe consequences for the original file system after sync’ing both locations anew, without the end user realizing this.
That’s were the Migration Dragon for SharePoint 2010 comes in. Its first and current incarnation is an experimental one and it’s based on a simple idea: what would happen if a tool leverages the SharePoint managed client object model to upload files to SharePoint? The client OM has the unique ability to batch commands, so it’s very possible that the results will be remarkably good.

So, combining that with my current preferences about what a migration tool should do, Migration Dragon for SharePoint 2010 offers the following features:

It allows you to specify the batch size of files that are to be uploaded. For example, if you specify a batch size of 20 MB, the tool will create a batch of files up to 20 MB and then upload that batch in one go before proceeding to the next batch.
It allows you to specify the max allowed batch size on the server. By default, the max allowed batch size is 3 MB for the client object model which isn’t a very useful amount. If you try to upload a batch of 20 MB of files, while the server only accepts 3 MB at a time, such requests will fail miserably.
It’s a tool that can be used by clients. Because it uses the SharePoint client object model, the tool doesn’t need to be executed on the server (except for the part that sets the server max batch size, that functionality is not available on a client because it leverages the SharePoint server object model).
The tool tracks which batches failed to upload correctly. After processing every batch, it will retry to upload failed batches. In order to do so, it 
switches strategy. Instead of uploading the entire failed batch, which will likely fail again, it will upload each file within the failed batch 
individually.
After retrying all failures, the tool provides feedback on the files that were unable to be uploaded, even after retrying.
It provides accurate updates about the progression informing the end user about the number of folders that have been processed and the amount of data that 
has been processed. One of the ways that the tool divulges this info is in the form of progress bars, so it’s made quite clear how much work there is still 
left to do. This is shown in the next Figure.



It translates most of the problematic characters/combinations in file and folder names. Problematic that is, for web environments. This includes the 
following tricky characters:  ~ # % & * : < > ? / { | }. I say most, because I’m not correcting some faulty combinations (such as a file name starting with dot (.), e.g. .MyFileName.txt).
Let’s take a look at the Migration Dragon for SharePoint 2010:



Max Batch Size

Let’s talk about the max batch size some more. The default max batch size in the tool is 10 MB, which means that all files on the file system will be packaged in a batch of up to 10 MB. That is, unless you have files that are larger than the max batch size (otherwise those files could never be uploaded). You can specify this here:



Since the default max batch size for the SharePoint client object model is quite small, 3 MB, it’s very likely that you want to change this. That is, if 
you want to gain any benefit from the batching mechanism. You need to increase this number to:

At least the size of the max batch size you want to use.
AND At least the size of the largest file you want to upload.  
You can change the server max batch size by clicking the Increase button: 



You can only execute this button when placing the tool on the server, as this part of the code uses the SharePoint server object model. Right now, if you click it on a client, the tool crashes badly. The rest of the tool can be used on any client. It executes the following code: