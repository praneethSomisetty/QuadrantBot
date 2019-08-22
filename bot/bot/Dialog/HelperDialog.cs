//make a quadrant about page
//make an email sending evite page
using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace bot.Dialogs
{
    public class HelperDialog : ComponentDialog
    {
        private const string initialText = "What would you like help with: \r\n 1. Timings & Locations \r\n 2. Contacts";

        public HelperDialog(LocationDialog locationdialog)
            : base(nameof(HelperDialog))
        {   

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(locationdialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                PromptsAsync,
                EndMessageAsync,
            }));
            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["dialog"] = new Properties();     
            var promptMessage = MessageFactory.Text(initialText, initialText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> PromptsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var x = stepContext.Context.Activity.Text;
            var y = (Properties)stepContext.Values["dialog"];
            y.Questionset1 = x;

            string message = string.Empty;
            if (x == "1" || x.Contains("timing", StringComparison.OrdinalIgnoreCase) || x.Contains("location", StringComparison.OrdinalIgnoreCase))
            {
                return await stepContext.BeginDialogAsync(nameof(LocationDialog), cancellationToken);
            }
            else if (x == "2" || x.Contains("contact", StringComparison.OrdinalIgnoreCase))
            {
                message = ("You can contact Vamshi Reddy at Vamshi@quadrantresource.com");               
            }
            message += "\r\n Do you want to continue? 1. Yes or 2. no";
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(message) }, cancellationToken);

        }

        private async Task<DialogTurnResult> LocationsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var x = stepContext.Context.Activity.Text;
            var y = (Properties)stepContext.Values["dialog"];
            y.Questionset2 = x;

            string message = string.Empty;
            if (x == "3" || x == "Hyderabad")
            {
                message = "SoftSol Building, 4th Floor,Tower 2, Inorbit Mall Road,Near Mindspace Flyover Junction,Hitech City, Madhapur,Hyderabad, Telanagana – 500081";
                message += "The timings for this location are unknown";
            }
            else if (x == "2" || x == "Texas")
            {
                message = "3333 Lee ParkwaySuite 600 Dallas, TX – 75219";
                message += "The timings for this location are monday to friday, 8am - 6pm";
            }
            else if (x == "1" || x == "Washington")
            {
                message = "4034 148th Ave NE, Suite K1C1, Redmond, WA 98052";
                message += "The timings for this location are monday to friday, 8am - 6pm";
            }
            else if (x == "4" || x == "Bangaluru")
            {
                message = "936, SCK complex, Second Floor,Channasandra, Kengeri main road,Rajarajeshwari Nagar,Bengaluru,Karnataka.";
                message += "The timings for this location are unknown";
            }
            message += "\r\n Do you want to continue? 1. Yes or 2. no";

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(message) }, cancellationToken);


        }

        private async Task<DialogTurnResult> EndMessageAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var y = (Properties)stepContext.Values["dialog"];

            if (stepContext.Context.Activity.Text == "yes" && !string.IsNullOrWhiteSpace(y.Questionset1))
            {
                return await stepContext.BeginDialogAsync(nameof(HelperDialog));
            }
            else
            {
                y.Questionset1 = string.Empty;
                return await stepContext.BeginDialogAsync(nameof(MainDialog));
            }

        }
    }
   
}
