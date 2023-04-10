using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CoreInvestment;
using Newtonsoft.Json;
using System.CommandLine.Parsing;

namespace InvestmentApiClient
{
    public class Common
    {
        public static Task<Client> GetClient(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption,
            InvocationContext context)
        {
            var username = context.ParseResult.GetValueForOption(userOption);
            var password = context.ParseResult.GetValueForOption(userPasswordOption);
            var url = context.ParseResult.GetValueForOption(baseUrlOption);
            var client = MakeClient(username, password, url);
            return client;
        }

        public static async Task<Client> MakeClient(string username, string password, string url)
        {
            var httpClient = new HttpClient();
            var client = new Client(url, httpClient);

            var userLoginInfo = new UserLoginInfo()
            {
                Username = username,
                Password = password
            };

            var tokenResponse = await client.TokenAsync(userLoginInfo);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenResponse.Token);

            return client;
        }

        public static void WriteJsonObject<T>(T obj, string label = null)
        {
            if (obj == null) return;
            Console.WriteLine((string.IsNullOrEmpty(label) ? "" : ": ") + JsonConvert.SerializeObject(obj));
        }

        public static Command CreateEntityCommand<T>(string commandName, string entitySingularName, string entityPluralName, Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption) where T : class 
            => new(commandName, $"Interact with {entityPluralName}")
            {
                ListCommand<T>(entityPluralName, userOption, userPasswordOption, baseUrlOption),
                DeleteCommand<T>(entitySingularName, userOption, userPasswordOption, baseUrlOption),
                GetCommand<T>(entitySingularName, userOption, userPasswordOption, baseUrlOption),
                UpdateCommand<T>(entitySingularName, userOption, userPasswordOption, baseUrlOption),
                CreateCommand<T>(userOption, userPasswordOption, baseUrlOption),
                //ReplaceCommand(userOption, userPasswordOption, baseUrlOption),
            };

        private static Operation CreateFieldReplacementOperation(string field, string value)
        {
            return new Operation()
            {
                Path = "/" + field,
                Value = value,
                Op = "replace",
                OperationType = OperationType.Replace
            };
        }

        public static async Task Update<T>(string? username, string? password, string? url, int id, string field,
            string value)
        {
            var client = await MakeClient(username, password, url);
            IEnumerable<Operation> body = new List<Operation>
            {
                CreateFieldReplacementOperation(field, value)
            };

            
            if (typeof(T) == typeof(RecordedActivity))
            {
                await client.ActivityPATCHAsync(id, body);
            }
            if (typeof(T) == typeof(CustomEntityType))
            {
                await client.CustomEntityTypePATCHAsync(id, body);
            }
            if (typeof(T) == typeof(CustomEntity))
            {
                await client.CustomEntityPATCHAsync(id, body);
            }
            if (typeof(T) == typeof(InvestmentInfluenceFactor))
            {
                await client.FactorPATCHAsync(id, body);
            }
            if (typeof(T) == typeof(Region))
            {
                await client.RegionPATCHAsync(id, body);
            }
            if (typeof(T) == typeof(InvestmentRisk))
            {
                await client.RiskPATCHAsync(id, body);
            }
            if (typeof(T) == typeof(InvestmentGroup))
            {
                await client.GroupPATCHAsync(id, body);
            }
            if (typeof(T) == typeof(InvestmentTransaction))
            {
                await client.TransactionPATCHAsync(id, body);
            }
            if (typeof(T) == typeof(Investment))
            {
                await client.InvestmentPATCHAsync(id, body);
            }
        }

        public static async Task Replace<T>(string? username, string? password, string? url, int id, T replacement)
        {
            var client = await MakeClient(username, password, url);
            if (typeof(T) == typeof(RecordedActivity))
            {
                await client.ActivityPUTAsync(id, replacement as RecordedActivity);
            }
            if (typeof(T) == typeof(CustomEntityType))
            {
                await client.CustomEntityTypePUTAsync(id, replacement as CustomEntityType);
            }
            if (typeof(T) == typeof(CustomEntity))
            {
                await client.CustomEntityPUTAsync(id, replacement as CustomEntity);
            }
            if (typeof(T) == typeof(InvestmentInfluenceFactor))
            {
                await client.FactorPUTAsync(id, replacement as InvestmentInfluenceFactor);
            }
            if (typeof(T) == typeof(Region))
            {
                await client.RegionPUTAsync(id, replacement as Region);
            }
            if (typeof(T) == typeof(InvestmentRisk))
            {
                await client.RiskPUTAsync(id, replacement as InvestmentRisk);
            }
            if (typeof(T) == typeof(InvestmentGroup))
            {
                await client.GroupPUTAsync(id, replacement as InvestmentGroup);
            }
            if (typeof(T) == typeof(InvestmentTransaction))
            {
                await client.TransactionPUTAsync(id, replacement as InvestmentTransaction);
            }
            if (typeof(T) == typeof(Investment))
            {
                await client.InvestmentPUTAsync(id, replacement as Investment);
            }
            
        }
        public static async Task Delete<T>(string? username, string? password, string? url, int id)
        {
            var client = await MakeClient(username, password, url);
            if (typeof(T) == typeof(RecordedActivity))
            {
                //await client.ActivityDELETEAsync(id);
            }
            if (typeof(T) == typeof(CustomEntityType))
            {
                await client.CustomEntityTypeDELETEAsync(id);
            }
            if (typeof(T) == typeof(CustomEntity))
            {
                await client.CustomEntityDELETEAsync(id);
            }
            if (typeof(T) == typeof(InvestmentInfluenceFactor))
            {
                await client.FactorDELETEAsync(id);
            }
            if (typeof(T) == typeof(Region))
            {
                await client.RegionDELETEAsync(id);
            }
            if (typeof(T) == typeof(InvestmentRisk))
            {
                await client.RiskDELETEAsync(id);
            }
            if (typeof(T) == typeof(InvestmentGroup))
            {
                await client.GroupDELETEAsync(id);
            }
            if (typeof(T) == typeof(InvestmentTransaction))
            {
                await client.TransactionDELETEAsync(id);
            }
            if (typeof(T) == typeof(Investment))
            {
                await client.InvestmentDELETEAsync(id);
            }
        }

        public static async Task<T?> Get<T>(string? username, string? password, string? url, int id) where T : class
        {
            var client = await MakeClient(username, password, url);
            T? result = default;

            if (typeof(T) == typeof(RecordedActivity))
            {
                result = await client.ActivityGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(CustomEntityType))
            {
                result =  await client.CustomEntityTypeGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(CustomEntity))
            {
                result =  await client.CustomEntityGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(InvestmentInfluenceFactor))
            {
                result = await client.RegionGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(Region))
            {
                result =  await client.RegionGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(InvestmentRisk))
            {
                result = await client.RiskGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(InvestmentGroup))
            {
                result =  await client.GroupGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(InvestmentTransaction))
            {
                result = await client.TransactionGETAsync(id).ConfigureAwait(false) as T;
            }
            if (typeof(T) == typeof(Investment))
            {
                result =  await client.InvestmentGETAsync(id).ConfigureAwait(false) as T;
            }

            return result;
        }

        public static async Task<T?> Create<T>(string? username, string? password, string? url, T entity) where T : class
        {
            var client = await MakeClient(username, password, url);
            T? result = default;

            if (typeof(T) == typeof(RecordedActivity))
            {
                result =  await client.ActivityPOSTAsync(entity as RecordedActivity) as T;
            }
            if (typeof(T) == typeof(CustomEntityType))
            {
                result =  await client.CustomEntityTypePOSTAsync(entity as CustomEntityType) as T;
            }
            if (typeof(T) == typeof(CustomEntity))
            {
                result =  await client.CustomEntityPOSTAsync(entity as CustomEntity) as T;
            }
            if (typeof(T) == typeof(InvestmentInfluenceFactor))
            {
                result =  await client.FactorPOSTAsync(entity as InvestmentInfluenceFactor) as T;
            }
            if (typeof(T) == typeof(Region))
            {
                result =  await client.RegionPOSTAsync(entity as Region) as T;
            }
            if (typeof(T) == typeof(InvestmentRisk))
            {
                result =  await client.RiskPOSTAsync(entity as InvestmentRisk) as T;
            }
            if (typeof(T) == typeof(InvestmentGroup))
            {
                result =  await client.GroupPOSTAsync(entity as InvestmentGroup) as T;
            }
            if (typeof(T) == typeof(InvestmentTransaction))
            {
                result =  await client.TransactionPOSTAsync(entity as InvestmentTransaction) as T;
            }
            if (typeof(T) == typeof(Investment))
            {
                result =  await client.InvestmentPOSTAsync(entity as Investment) as T;
            }

            return result;
        }

        public static async Task<ICollection<T>> List<T>(string username, string password, string url)
        {
            var client = await MakeClient(username, password, url);
            ICollection<T?>? result = default;

            if (typeof(T) == typeof(CustomEntityType))
            {
                result = (ICollection<T?>?) await client.CustomEntityTypeAllAsync();
            }
            if (typeof(T) == typeof(CustomEntity))
            {
                result =  (ICollection<T?>?) await client.CustomEntityAllAsync();
            }
            if (typeof(T) == typeof(InvestmentInfluenceFactor))
            {
                result = (ICollection<T?>?)await client.FactorAllAsync();
            }
            if (typeof(T) == typeof(Region))
            {
                result = (ICollection<T?>?)await client.RegionAllAsync();
            }
            if (typeof(T) == typeof(InvestmentRisk))
            {
                result = (ICollection<T?>?)await client.RiskAllAsync();
            }
            if (typeof(T) == typeof(InvestmentGroup))
            {
                result = (ICollection<T?>?)await client.GroupAllAsync();
            }
            if (typeof(T) == typeof(InvestmentTransaction))
            {
                result = (ICollection<T?>?)await client.TransactionAllAsync();
            }
            if (typeof(T) == typeof(Investment))
            {
                result = (ICollection<T?>?)await client.InvestmentAllAsync();
            }

            return result ?? throw new NotImplementedException($"List function not implemented for {typeof(T).Name}");
        }

        public static Command ListCommand<T>(string entityPluralName, Option<string> option, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var getAllEntities = new Command("List", $"Lists all {entityPluralName}");

            getAllEntities.SetHandler(async (username, password, url) =>
            {
                var entities = await List<T>(username, password, url);;

                foreach (var investment in entities)
                {
                    System.Console.WriteLine();
                    WriteJsonObject(investment);
                }
            }, option, userPasswordOption, baseUrlOption);

            return getAllEntities;
        }

        public static Command DeleteCommand<T>(string entitySingularName, Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var idArg = new Argument<int>("Id", $"{entitySingularName} Id");
            var deleteEntityCommand = new Command("Delete", $"Deletes an {entitySingularName}")
            {
                idArg
            };

            deleteEntityCommand.SetHandler(async (context) =>
            {
                var username = context.ParseResult.GetValueForOption(userOption);
                var password = context.ParseResult.GetValueForOption(userPasswordOption);
                var url = context.ParseResult.GetValueForOption(baseUrlOption);
                var id = context.ParseResult.GetValueForArgument(idArg);
                await Delete<T>(username, password, url, id);
            });

            return deleteEntityCommand;
        }

        public static Command GetCommand<T>(string entitySingularName, Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption) where T : class
        {
            var idArg = new Argument<int>("Id", $"{entitySingularName} Id");
            var getEntityCommand = new Command("Get", $"Get an {entitySingularName}")
            {
                idArg
            };

            getEntityCommand.SetHandler(async context =>
            {
                var username = context.ParseResult.GetValueForOption(userOption);
                var password = context.ParseResult.GetValueForOption(userPasswordOption);
                var url = context.ParseResult.GetValueForOption(baseUrlOption);
                
                var id = context.ParseResult.GetValueForArgument(idArg);
                var result = await Get<T>(username, password, url, id);
                WriteJsonObject(result);
            });

            return getEntityCommand;
        }

        public static Command UpdateCommand<T>(string entitySingularName, Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var fieldArg = new Argument<string>("Field", $"Field on the {entitySingularName} to update/change");
            var valuedArg = new Argument<string>("Value", "New value for specified field.");
            var idArg = new Argument<int>("Id", "Investment Id");

            var updateEntityCommand = new Command("Update", $"Updates an {entitySingularName}")
            {
                idArg,
                fieldArg,
                valuedArg
            };

            updateEntityCommand.SetHandler(async context =>
            {
                var id = context.ParseResult.GetValueForArgument(idArg);
                var field = context.ParseResult.GetValueForArgument(fieldArg);
                var value = context.ParseResult.GetValueForArgument(valuedArg);

                var username = context.ParseResult.GetValueForOption(userOption);
                var password = context.ParseResult.GetValueForOption(userPasswordOption);
                var url = context.ParseResult.GetValueForOption(baseUrlOption);
                
                await Update<T>(username, password, url, id, field, value);
            });

            return updateEntityCommand;
        }

        private static Command CreateCommand<T>(Option<string> userOption, Option<string> userPasswordOption,
            Option<string> baseUrlOption)
        {
            if (typeof(T) == typeof(InvestmentInfluenceFactor))
            {
                return CreateFactorCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(InvestmentRisk))
            {
                return CreateRiskCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(Region))
            {
                return CreateRegionCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(InvestmentGroup))
            {
                return CreateGroupCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(InvestmentTransaction))
            {
                return CreateTransactionCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(CustomEntity))
            {
                return CreateCustomEntityCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(CustomEntityType))
            {
                return CreateCustomEntityTypeCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(RecordedActivity))
            {
                return CreateActivityCommand(userOption, userPasswordOption, baseUrlOption);
            }
            if (typeof(T) == typeof(InvestmentNote))
            {
                return CreateNoteCommand(userOption, userPasswordOption, baseUrlOption);
            }
            throw new NotImplementedException();
        }

        private static Command CreateFactorCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of investment");
            var influenceArg = new Argument<string>("Influence", "Description how how this factor influences the investment");
            var descriptionOpt = new Option<string>("Description", "Description of investment");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the investment should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "Investment points/weighting");

            var createFactorCommand = new Command("Create", "Create an factor")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption,
                influenceArg
            };

            createFactorCommand.SetHandler(async context =>
            {
                var factor = new InvestmentInfluenceFactor()
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                    Influence = context.ParseResult.GetValueForArgument(influenceArg)
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), factor);
                WriteJsonObject(createdResult);
            });

            return createFactorCommand;
        }

        private static Command CreateRiskCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of risk");
            var riskTypeArg = new Argument<RiskType>("RiskType", "Type of risk");
            var descriptionOpt = new Option<string>("Description", "Description of risk");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the risk should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "Risk points/weighting");

            var createRiskCommand = new Command("Create", "Create an risk")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption,
                riskTypeArg
            };

            createRiskCommand.SetHandler(async context =>
            {
                var risk = new InvestmentRisk()
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                    Type = context.ParseResult.GetValueForArgument(riskTypeArg),
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), risk);
                WriteJsonObject(createdResult);
            });

            return createRiskCommand;
        }

        private static Command CreateRegionCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of region");
            var descriptionOpt = new Option<string>("Description", "Description of region");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the region should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "region points/weighting");

            var createRegionCommand = new Command("Create", "Create an region")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption
            };

            createRegionCommand.SetHandler(async context =>
            {
                var region = new Region
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), region);
                WriteJsonObject(createdResult);
            });

            return createRegionCommand;
        }

        private static Command CreateGroupCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of group");
            var descriptionOpt = new Option<string>("Description", "Description of group");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the group should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "group points/weighting");
            var typeOption = new Option<string>("Type", "Type of group");

            var createRegionCommand = new Command("Create", "Create an group")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption
            };

            createRegionCommand.SetHandler(async context =>
            {
                var group = new InvestmentGroup()
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                    Type = context.ParseResult.GetValueForOption(typeOption),
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), group);
                WriteJsonObject(createdResult);
            });

            return createRegionCommand;
        }

        private static Command CreateTransactionCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of transaction");
            var descriptionOpt = new Option<string>("Description", "Description of transaction");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the transaction should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "transaction points/weighting");
            var commissionArg = new Argument<double>("Commission","Amount of commission");
            var currencyArg = new Argument<string>("Currency", "currency");
            var numUnitsArgs = new Argument<int>("Units", "Number of units");
            var pricePerUnitArg = new Argument<float>("PricePerUnit", "Price per unit");
            var transactionTypeArg = new Argument<string>("Transaction type", "Transaction type");
            var transactionDateOption = new Option<DateTimeOffset>("TransactionDate", "Date of transaction");
            var investmentIdArg = new Argument<int>("Investment Id", "The investment Id that this transaction is for");
            var createRegionCommand = new Command("Create", "Create an transaction")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption,
                commissionArg,
                investmentIdArg,
                currencyArg,
                numUnitsArgs,
                pricePerUnitArg,
                transactionTypeArg,
                transactionDateOption,
            };

            createRegionCommand.SetHandler(async context =>
            {

                var txn = new InvestmentTransaction()
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                    Commission = context.ParseResult.GetValueForArgument(commissionArg),
                    Currency = context.ParseResult.GetValueForArgument(currencyArg),
                    NumUnits = context.ParseResult.GetValueForArgument(numUnitsArgs),
                    PricePerUnit = context.ParseResult.GetValueForArgument(pricePerUnitArg),
                    TransactionType = context.ParseResult.GetValueForArgument(transactionTypeArg),
                    TransactionDate = context.ParseResult.GetValueForOption(transactionDateOption),
                    InvestmentId = context.ParseResult.GetValueForArgument(investmentIdArg)
                    
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), txn);
                WriteJsonObject(createdResult);
            });

            return createRegionCommand;
        }

        private static Command CreateCustomEntityCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of custom entity");
            var descriptionOpt = new Option<string>("Description", "Description of custom entity");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the custom entity should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "custom entity points/weighting");
            var customEntityTypeArg = new Argument<int?>("CustomEntityTypeId", ()=> null, "Id of Custom Entity Type");
            var owningEntityIdArg = new Argument<int>("OwningEntityId", "Id of the owning entity");
            var owningEntityTypeArg = new Argument<EntityType>("OwningEntityType", "Owning Entity Type");
            var owningCustomEntityOption = new Option<int?>("OwningCustomEntityId", ()=> null,
                "owning custom entity if owning entityType = custom");
            var createRegionCommand = new Command("Create", "Create an custom entity")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption,
                customEntityTypeArg,
                owningEntityIdArg,
                owningEntityTypeArg,
                owningCustomEntityOption
            };

            createRegionCommand.SetHandler(async context =>
            {
                var username = context.ParseResult.GetValueForOption(userOption);
                var password = context.ParseResult.GetValueForOption(userPasswordOption);
                var url = context.ParseResult.GetValueForOption(baseUrlOption);
                var customEntityTypeId = context.ParseResult.GetValueForArgument(customEntityTypeArg);
                var owningEntityType = context.ParseResult.GetValueForArgument(owningEntityTypeArg);
                var owningCustomEntityId = context.ParseResult.GetValueForOption(owningCustomEntityOption);

                var entity = new CustomEntity
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                    OwningEntityId = context.ParseResult.GetValueForArgument(owningEntityIdArg),

                    // Investment Id, Risk id or CustomEntityType Id etc.
                    OwningEntityType = owningEntityType,

                    // Is this Custom Entity for a user defined type eg. Products, Services, Business Segment, Investment Review? OK, get it...
                    CustomEntityType = customEntityTypeId.HasValue 
                        ? await Get<CustomEntityType>(username, password, url, customEntityTypeId.Value)
                        : null,
                    // Owner is a custom entity? OK, get it...
                    OwningCustomEntity = owningCustomEntityId.HasValue && owningEntityType == EntityType.Custom
                        ? await Get<CustomEntity>(username, password, url, owningCustomEntityId.Value)
                        : null,
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption),
                    context.ParseResult.GetValueForOption(baseUrlOption), entity);
                WriteJsonObject(createdResult);
            });

            return createRegionCommand;
        }

        private static Command CreateCustomEntityTypeCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of custom entity type");
            var descriptionOpt = new Option<string>("Description", "Description of custom entity type");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the custom entity type should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "custom entity type points/weighting");
            var dataTypeArg = new Argument<EntityType>("DataType", "Data type of the custom entity type");
            
            var createRegionCommand = new Command("Create", "Create an custom entity type")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption,
                dataTypeArg
            };

            createRegionCommand.SetHandler(async context =>
            {
                var entity = new CustomEntityType()
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                    DataType = context.ParseResult.GetValueForArgument(dataTypeArg),
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), 
                    context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), entity );
                WriteJsonObject(createdResult);
            });

            return createRegionCommand;
        }

        private static Command CreateNoteCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of note");
            var descriptionOpt = new Option<string>("Description", "Description of note");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the note should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "note points/weighting");
            var createRegionCommand = new Command("Create", "Create an note")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption,
            };

            createRegionCommand.SetHandler(async context =>
            {
                var note = new InvestmentNote()
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),

                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), note);
                WriteJsonObject(createdResult);
            });

            return createRegionCommand;
        }

        private static Command CreateActivityCommand(Option<string> userOption, Option<string> userPasswordOption, Option<string> baseUrlOption)
        {
            var nameArg = new Argument<string>("Name", "Name of activity");
            var descriptionOpt = new Option<string>("Description", "Description of activity");
            var isFlaggedOption = new Option<bool>("IsFlagged", "Set if the activity should be highlisghted/flagged");
            var pointsOption = new Option<long>("Points", "activity points/weighting");
            var detailsOption = new Option<string>("Details", "details");
            var tagOption = new Option<string>("Tag", "Some tag");

            var createRegionCommand = new Command("Create", "Create an activity")
            {
                nameArg,
                descriptionOpt,
                isFlaggedOption,
                pointsOption,
                detailsOption,
                tagOption
            };

            createRegionCommand.SetHandler(async context =>
            {
                var group = new RecordedActivity()
                {
                    Name = context.ParseResult.GetValueForArgument(nameArg),
                    Description = context.ParseResult.GetValueForOption(descriptionOpt),
                    IsFlagged = context.ParseResult.GetValueForOption(isFlaggedOption),
                    Points = context.ParseResult.GetValueForOption(pointsOption),
                    Details = context.ParseResult.GetValueForOption(detailsOption),
                    Tag = context.ParseResult.GetValueForOption(tagOption)
                };
                var createdResult = await Create(context.ParseResult.GetValueForOption(userOption), context.ParseResult.GetValueForOption(userPasswordOption), context.ParseResult.GetValueForOption(baseUrlOption), group);
                WriteJsonObject(createdResult);
            });

            return createRegionCommand;
        }
    }
}
