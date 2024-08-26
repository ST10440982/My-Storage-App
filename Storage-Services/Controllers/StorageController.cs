using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using Azure;

public class StorageController : Controller
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly QueueServiceClient _queueServiceClient;
    private readonly ShareServiceClient _shareServiceClient;

    public StorageController(BlobServiceClient blobServiceClient, QueueServiceClient queueServiceClient, ShareServiceClient shareServiceClient)
    {
        _blobServiceClient = blobServiceClient;
        _queueServiceClient = queueServiceClient;
        _shareServiceClient = shareServiceClient;
    }

    // Upload image to Azure Blob Storage and queue a message
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("product-images");
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(file.FileName);
        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        var queueClient = _queueServiceClient.GetQueueClient("orders-queue");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.SendMessageAsync($"Uploaded image {file.FileName}");

        return RedirectToAction("Index");
    }

    // Add order processing message to Azure Queue
    [HttpPost]
    public async Task<IActionResult> ProcessOrder(string orderId)
    {
        var queueClient = _queueServiceClient.GetQueueClient("orders-queue");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.SendMessageAsync($"Processing order {orderId}");

        return RedirectToAction("Index");
    }

    // Upload contract file to Azure Files
    [HttpPost]
    public async Task<IActionResult> UploadContract(IFormFile file)
    {
        var shareClient = _shareServiceClient.GetShareClient("contracts");
        await shareClient.CreateIfNotExistsAsync();

        var directoryClient = shareClient.GetRootDirectoryClient();
        var fileClient = directoryClient.GetFileClient(file.FileName);

        await using (var stream = file.OpenReadStream())
        {
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadRangeAsync(new HttpRange(0, stream.Length), stream);
        }

        return RedirectToAction("Index");
    }
}
