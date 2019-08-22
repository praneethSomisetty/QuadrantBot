

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace bot.Dialogs
{
    public class ApplicationDialog : ComponentDialog
    {
        private const string InitialText = "What position would you like to apply for";
        private const string RepromptText = "Sorry, please enter a valid position that is avaliable.";

        public ApplicationDialog()
            : base(nameof(ApplicationDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                InitialStepAsync,
                EndMessageAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var promptMessage = MessageFactory.Text(InitialText, InitialText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            
            var option = stepContext.Context.Activity.Text;

           
            var m = ("If you would like to apply for the " + option + " position, here is the link to the application: http://www.quadrantresource.com/careers \r\n Would you like to continue? : Yes or No");
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(m) }, cancellationToken);

        }

        private async Task<DialogTurnResult> EndMessageAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            if (stepContext.Context.Activity.Text == "yes")
            {
                return await stepContext.BeginDialogAsync(nameof(ApplicationDialog));
            }
            else
            {
                return await stepContext.BeginDialogAsync(nameof(MainDialog));
            }

        }
    }
}
