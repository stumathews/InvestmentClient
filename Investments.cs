using System.CommandLine;
using CoreInvestment;
using static InvestmentApiClient.Common;

namespace InvestmentApiClient
{
    public class Investments
    {
        public static Command CreateInvestmentsCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            const string investmentsSingularName = "investment";
            const string investmentsPluralName = investmentsSingularName + "s";

            return new("Investments", "Interact with investments")
            {
                ListCommand<Investment>(investmentsPluralName, userOption, userPasswordOption, baseUrlOption),
                DeleteCommand<Investment>(investmentsSingularName, userOption, userPasswordOption, baseUrlOption),
                GetCommand<Investment>(investmentsSingularName, userOption, userPasswordOption, baseUrlOption),
                UpdateCommand<Investment>(investmentsSingularName, userOption, userPasswordOption, baseUrlOption),
                ReplaceCommand(userOption, userPasswordOption, baseUrlOption),
                CreateCommand(userOption, userPasswordOption, baseUrlOption)
            };
        }

        private static Command ReplaceCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var idArg = new Argument<int>("Id", "Investment Id");
            var nameArg = new Argument<string>("Name", "Change name of investment");
            
            var replaceCommand = new Command("Replace", "Replace and entire investment")
            {
                idArg,
                nameArg
            };

            replaceCommand.SetHandler(async context =>
            {
                var username = context.ParseResult.GetValueForOption(userOption);
                var password = context.ParseResult.GetValueForOption(userPasswordOption);
                var url = context.ParseResult.GetValueForOption(baseUrlOption);
                var id = context.ParseResult.GetValueForArgument(idArg);
                var name = context.ParseResult.GetValueForArgument(nameArg);

                var replacement = new Investment
                {
                    Id = id,
                    Name = name,
                    Description = "This replaced the previous investment"
                };

                await ReplaceInvestment(username, password, url, id, replacement);
            });

            return replaceCommand;
        }
        
        private static Command CreateCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of investment");
            var descriptionOpt = new Option<string>("Description", "Description of investment");
            var symbolArg = new Argument<string>("Symbol", "Description of investment");
            var currencyOpt = new Option<double>("Currency", "Currency conversion from base currency");
            var desirabilityStatementOption = new Option<string>("DesirabilityStatement", "Why the investment is desirable");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the investment should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "Investment points/weighting");
            var createInvestmentCommand = new Command("Create", "Create an investment")
            {
                nameArg,
                descriptionOpt,
                symbolArg,
                currencyOpt,
                desirabilityStatementOption,
                isFlaggedOption,
                pointsOption
            };

            createInvestmentCommand.SetHandler(async context =>
            {
                var username = context.ParseResult.GetValueForOption(userOption);
                var password = context.ParseResult.GetValueForOption(userPasswordOption);
                var url = context.ParseResult.GetValueForOption(baseUrlOption);
                var name = context.ParseResult.GetValueForArgument(nameArg);
                var description = context.ParseResult.GetValueForOption(descriptionOpt);
                var symbol = context.ParseResult.GetValueForArgument(symbolArg);
                var currency = context.ParseResult.GetValueForOption(currencyOpt);
                var desirabilityStatement = context.ParseResult.GetValueForOption(desirabilityStatementOption);
                var isFlagged = context.ParseResult.GetValueForOption(isFlaggedOption);
                var points = context.ParseResult.GetValueForOption(pointsOption);

                var investment = new Investment
                {
                    Name = name,
                    Description = description,
                    Symbol = symbol,
                    Currency = currency,
                    DesirabilityStatement = desirabilityStatement,
                    IsFlagged = isFlagged,
                    Points = points,
                    // Value = 
                    // ValueProposition = 
                    // CustomEntities = 
                    // Factors = 
                    // Groups = 
                    // Id = 
                    // InitialInvestment = 
                    // InvestmentIds = 
                    // CreatedTime = 
                    // LastModifiedTime = 
                    // Regions = 
                    // Transactions = 

                };
                var createdResult = await Create(username, password, url, investment);
                WriteJsonObject(createdResult);
            });

            return createInvestmentCommand;
        }

        public static async Task<ICollection<Investment>> ListInvestments(string username, string password, string url) 
            => await List<Investment>(username, password, url);

        public static async Task<Investment?> CreateInvestment(string? username, string? password, string? url, Investment? investment) 
            => await Create<Investment>(username, password, url, investment);

        public static async Task DeleteInvestment(string? username, string? password, string? url, int id) 
            => await Delete<Investment>(username, password, url, id);

        public static async Task<Investment?> GetInvestment(string? username, string? password, string? url, int id) 
            => await Get<Investment>(username, password, url, id);

        public static async Task UpdateInvestment(string? username, string? password, string? url, int id, string field, string value) 
            => await Update<Investment>(username, password, url, id, field, value).ConfigureAwait(false);

        public static async Task ReplaceInvestment(string? username, string? password, string? url, int id, Investment replacement) 
            => await Replace(username, password, url, id, replacement).ConfigureAwait(false);
    }
}
