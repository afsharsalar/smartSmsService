// See https://aka.ms/new-console-template for more information

using SmartSmsService.Model;

try
{
    var result =
        await SmartSmsService.Service.SmsWebService.SimpleSend(11, "11", new List<SimpleSmsSendRequestModel>
        {
            new SimpleSmsSendRequestModel
            {
                Message = "تست از سرویس جدید",
                Mobile = 111111,
                Originator = "11111"

            }
        });
}
catch (Exception)
{

	throw;
}

Console.WriteLine("Hello, World!");
