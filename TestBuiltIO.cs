using System;
using System.Collections.Generic;
using BuiltSDK;
using BettanDataHandler;

namespace Testing
{
	public class DummyModelExternalData
	{
		public string ID;
		public string FirstName;
		public string LastName;
		public string Company;
		public string Email;
	}

	public class DummyModelBuiltIO
	{
		public string first_name;
		public string last_name;
		public string company;
		public string external_id;
	}

	public class TestBuiltIO
	{
		private static string BuiltIO_API_Key = Helper.AppSetting("BuiltIO_API_Key");
		private static string BuiltIO_AppID = Helper.AppSetting("BuiltIO_AppID");
		private static string BuiltIO_User = Helper.AppSetting("BuiltIO_Importer_User");
		private static string BuiltIO_Pass = Helper.AppSetting("BuiltIO_Importer_Pass");

		private static DummyModelExternalData[] TestData = {
			new DummyModelExternalData() { 
				ID = "TEST01", 
				FirstName = "Larry", 
				LastName = "Page", 
				Company = "Google", 
				Email = "larry.page@gmail.com" 
			},
			new DummyModelExternalData() { 
				ID = "TEST02", 
				FirstName = "Mark", 
				LastName = "Zuckerberg", 
				Company = "Facebook", 
				Email = "mark.zuckerberg@facebook.com" 
			},
			new DummyModelExternalData() { 
				ID = "TEST03", 
				FirstName = "Neha", 
				LastName = "Sampat", 
				Company = "Built.io", 
				Email = "neha.sampat@built.io" 
			}
		};


		public static void Import()
		{
			Built.initialize(BuiltIO_API_Key, BuiltIO_AppID);

			// Login with importer account (which is included in "Data Importers" role group)
			var user = new BuiltUser().login(BuiltIO_User, BuiltIO_Pass);


			foreach(var item in TestData) {

				/* Using the Built.IO class with uid "test_class", 
				 * containing following (text) fields: first_name, last_name, company, external_id
				 */
				var bioItem = new BuiltObject<DummyModelBuiltIO>("test_class");

				bioItem.data = new DummyModelBuiltIO() {
					first_name = item.FirstName,
					last_name = item.LastName,
					company = item.Company,
					external_id = item.ID,
				};

				// update if external id exists, or else create
				/*bioItem.upsert(new Dictionary<string, object>() {
					{ "external_id", item.ID }
				});*/

				bioItem.save(); // throws (422) Unprocessable Entity 
			}

			user.logout();
		}
	}
}

