using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk;

namespace MySamples
{
    public class BasicPushSMS : IPlugin
    {
        string smsapi = ""; // API from SMS Gateway
        string title =  "SMS From CRM";
        string from = "0000" // set number based on short/longcode/instruction from SMS provider
        string to = "";
        string message = "";


        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            if (context.Depth > 1) return;



            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                Entity PreImage = (Entity)context.PreEntityImages["PreUpdateImage1"]; // Advisable to send sms only when confirmation has been made through an update of the rocord


                
                    if (entity.LogicalName == "new_sms" && entity.Contains("new_sendnow") && entity.GetAttributeValue<bool>("new_sendnow")
                    {
                        
                        EntityReference receiverguid = (EntityReference)PreImage.Attributes["new_to"]; // retreive contact refernece
                        Entity receiver = service.Retrieve("contact", contact.Id, new ColumnSet(true)); // retreive the entity itself
                        to = receiver.Attributes["new_to"].ToString(); // set the phone number on the Contact entity
                        message = PreImage.Attributes["new_body"].ToString(); // set the message
                            

                        BasicHttpBinding myBinding = new BasicHttpBinding();
                        myBinding.Name = "BasicHttpBinding_IService1"; // name of active binding in webconfig
                        myBinding.Security.Mode = BasicHttpSecurityMode.None;
                        myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                        myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                        myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                        EndpointAddress endPointAddress = new EndpointAddress(smsapi);


                        
                        myClient = new YourClientXXXX(myBinding, endPointAddress) // initialize client
                        myClient.Send(from, title, to, message); // where Send is name of method from SMS Provider
                        //set binding attributes depending on API performance
                        myBinding.CloseTimeout = TimeSpan.MaxValue;
                        myBinding.OpenTimeout = TimeSpan.MaxValue;
                        myBinding.ReceiveTimeout = TimeSpan.MaxValue;
                        myBinding.SendTimeout = TimeSpan.MaxValue;
                        
                    }

            }
        }
    }
}
