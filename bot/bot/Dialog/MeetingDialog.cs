
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace bot.Dialogs
{
    public class MeetingDialog : ComponentDialog
    {
        private const string Nametext = "What is your name";
        private const string Emailtext = "Where what is your email?";
        private const string Phonenumbertext = "What is your phone?";
        private const string Meetingtimetext = "What is the best time to meet?";

        public MeetingDialog()
            : base(nameof(MeetingDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                NameStepAsync,
                EmailStepAsync,
                PhonenumberStepAsync,
                DateandTimeStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var message = "Can you provide the below informations";
            message += "\r\n" + Nametext;
            var promptMessage = MessageFactory.Text(message, Nametext, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> EmailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var Details = (MeetingInformation)stepContext.Values[""];

            Details.Name = (string)stepContext.Result;

            if (Details.email == null)
            {
                var promptMessage = MessageFactory.Text(Emailtext, Emailtext, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(Details.email, cancellationToken);
        }

        private async Task<DialogTurnResult> PhonenumberStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var Details = (MeetingInformation)stepContext.Options;

            Details.email = (string)stepContext.Result;

            if (Details.phonenumber == null)
            {
                var promptMessage = MessageFactory.Text(Phonenumbertext, Phonenumbertext, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);

            }

            return await stepContext.NextAsync(Details.phonenumber, cancellationToken);
        }
        private async Task<DialogTurnResult> DateandTimeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var Details = (MeetingInformation)stepContext.Options;

            Details.phonenumber = (string)stepContext.Result;

            if (Details.phonenumber == null)
            {
                var promptMessage = MessageFactory.Text(Meetingtimetext, Meetingtimetext, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);

            }

            return await stepContext.NextAsync(Details.phonenumber, cancellationToken);
        }


        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var Details = (MeetingInformation)stepContext.Options;

            Details.dateandtime = (string)stepContext.Result;

            var messageText = $"Please confirm your details: \r\n name: {Details.Name} \r\n Email:{Details.email} \r\n phonenumber: {Details.phonenumber} \r\n Meeting date and time: {Details.dateandtime} \r\n Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
