using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureQueueObject;
using Newtonsoft.Json;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=anishstorage786;AccountKey=E0ShJ37KOKnzUF5p5ijvLKigF1Tm02OJPsKQ0UqA+ouiwPZ+LC0oMXt56Ky4Vg0dpaJdwiIA8UT++ASt4lT15Q==;EndpointSuffix=core.windows.net";
string queueName = "anishqueue";

//await PeekMessages();

// send message 
await SendMessage("1",786);
await SendMessage("2",92);

async Task SendMessage(string orderid, int quantity)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    if (queueClient.Exists())
    {
        Order order = new Order { OrderID = orderid, Quantity = quantity };
        var jsonObject = JsonConvert.SerializeObject(order);
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonObject);
        var message = System.Convert.ToBase64String(bytes);
        await queueClient.SendMessageAsync(message); // JsonConvert.SerializeObject(order)
        Console.WriteLine("Order Id {0} sent", orderid);
    }
}

async Task PeekMessages()
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    int maxMessages = 10;

    if (queueClient.Exists())
    {
        PeekedMessage[] peekedMessages = await queueClient.PeekMessagesAsync(maxMessages);
        Console.WriteLine("The orders in the queue");
        foreach (var message in peekedMessages)
        {
            Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
            Console.WriteLine("Order Id {0}", order.OrderID);
            Console.WriteLine("Order Quantity {0}", order.Quantity);
        }
    }
}
