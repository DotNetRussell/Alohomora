using Alohomora.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alohomora.Utilities
{
    public static class DatabaseStuff
    {
        private static SqlConnection _connection { get; set; }
        public static void InitializeDB()
        {
            Console.WriteLine("Connecting to db...");
            _connection = new SqlConnection(ApplicationConfiguration.ConnectionString);
        }

        private static string buildQuery(string fname, string lname, DateTime dob, bool equal, bool after,
                                                    bool before, string streetAddress, string city, string zip)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select distinct * from (select LAST_NAME, FIRST_NAME, DATE_OF_BIRTH, RESIDENTIAL_ADDRESS1, RESIDENTIAL_CITY from " + ApplicationConfiguration.DatabaseTableName +" where ");
            if (!String.IsNullOrEmpty(fname))
            {
                builder.Append("FIRST_NAME = @fname ");
            }
            if (!String.IsNullOrEmpty(lname))
            {
                if (builder.ToString().Contains("@"))
                {
                    builder.Append("AND LAST_NAME = @lname ");
                }
                else
                {
                    builder.Append("LAST_NAME = @lname ");
                }
            }
            if (dob != DateTime.MinValue)
            {
                if (builder.ToString().Contains("@"))
                {
                    if (equal)
                    {
                        builder.Append("AND DATE_OF_BIRTH = @dob ");
                    }
                    else if (after)
                    {
                        builder.Append("AND DATE_OF_BIRTH > @dob ");
                    }
                    else
                    {
                        builder.Append("AND DATE_OF_BIRTH < @dob ");
                    }

                }
                else
                {
                    if (equal)
                    {
                        builder.Append("DATE_OF_BIRTH = @dob ");
                    }
                    else if (after)
                    {
                        builder.Append("DATE_OF_BIRTH > @dob ");
                    }
                    else
                    {
                        builder.Append("DATE_OF_BIRTH < @dob ");
                    }

                }
            }
            if (!String.IsNullOrEmpty(streetAddress))
            {
                if (builder.ToString().Contains("@"))
                {
                    builder.Append("AND RESIDENTIAL_ADDRESS1 LIKE @raddress ");
                }
                else
                {
                    builder.Append("RESIDENTIAL_ADDRESS1 LIKE @raddress ");
                }
            }
            if (!String.IsNullOrEmpty(city))
            {
                if (builder.ToString().Contains("@"))
                {
                    builder.Append("AND RESIDENTIAL_CITY = @city ");
                }
                else
                {
                    builder.Append("RESIDENTIAL_CITY = @city ");
                }
            }
            if (!String.IsNullOrEmpty(zip))
            {
                if (builder.ToString().Contains("@"))
                {
                    builder.Append("AND RESIDENTIAL_ZIP = @zip");
                }
                else
                {
                    builder.Append("RESIDENTIAL_ZIP = @zip");
                }
            }

            builder.Append(") ss");
            return builder.ToString();
        }

        private static SqlCommand BuildCommand(string paramString, string fname, string lname, DateTime dob, bool equal, bool after,
                                                    bool before, string streetAddress, string city, string zip)
        {
            SqlCommand retCommand = new SqlCommand(paramString, _connection);

            if (!String.IsNullOrEmpty(fname))
            {
                retCommand.Parameters.AddWithValue("@fname", fname);
            }
            if (!String.IsNullOrEmpty(lname))
            {
                retCommand.Parameters.AddWithValue("@lname", lname);
            }
            if (dob != DateTime.MinValue)
            {
                retCommand.Parameters.AddWithValue("@dob", dob.ToShortDateString());
            }
            if (!String.IsNullOrEmpty(streetAddress))
            {
                retCommand.Parameters.AddWithValue("@raddress", streetAddress);
            }
            if (!String.IsNullOrEmpty(city))
            {
                retCommand.Parameters.AddWithValue("@city", city);
            }
            if (!String.IsNullOrEmpty(zip))
            {
                retCommand.Parameters.AddWithValue("@zip", zip);
            }

            return retCommand;
        }


        public static async Task<List<PersonModel>> RunQuery(string fname, string lname, string city)
        {
            List<PersonModel> targets = new List<PersonModel>();
            try
            {
                InitializeDB();
                string _sqlCommand = ("select distinct * from (select LAST_NAME, FIRST_NAME, DATE_OF_BIRTH, RESIDENTIAL_ADDRESS1, RESIDENTIAL_CITY from " + ApplicationConfiguration.DatabaseTableName + " where RESIDENTIAL_CITY = @city AND FIRST_NAME = @fname AND LAST_NAME = @lname) ss");
                await _connection.OpenAsync();
                SqlCommand command = new SqlCommand(_sqlCommand, _connection);
                command.Parameters.AddWithValue("@fname", fname);
                command.Parameters.AddWithValue("@lname", lname);
                command.Parameters.AddWithValue("@city", city);
                Console.WriteLine("Querying Database for users...");
                SqlDataReader dataReader = await command.ExecuteReaderAsync();
                int count = 0;
                while (dataReader.Read())
                {
                    count++;
                    PersonModel temp = new PersonModel()
                    {
                        Id = Guid.NewGuid(),
                        lastname = dataReader.GetValue(0).ToString(),
                        firstname = dataReader.GetValue(1).ToString(),
                        dob = dataReader.GetValue(2).ToString(),
                        address = dataReader.GetValue(3).ToString(),
                        city = dataReader.GetValue(4).ToString(),
                        state = ApplicationConfiguration.DatabaseState
                    };

                    targets.Add(temp);
                }
                dataReader.Close();
                command.Dispose();
                _connection.Close();
                Console.WriteLine("ROWS: " + count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); /**nom nom nom  **/
            }

            return targets;
        }

        public static async Task<List<PersonModel>> RunQuery(string fname, string lname, DateTime dob, bool equal, bool after,
                                                bool before, string streetAddress, string city, string zip)
        {
            List<PersonModel> targets = new List<PersonModel>();
            try
            {
                InitializeDB();
                string _sqlCommand = buildQuery(fname, lname, dob, equal, after, before, streetAddress, city, zip);

                _connection.Open();
                SqlCommand command = new SqlCommand(_sqlCommand, _connection);

                if (!String.IsNullOrEmpty(fname))
                {
                    command.Parameters.AddWithValue("@fname", fname);
                }
                if (!String.IsNullOrEmpty(lname))
                {
                    command.Parameters.AddWithValue("@lname", lname);
                }
                if (dob != DateTime.MinValue)
                {
                    command.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-DD"));
                }
                if (!String.IsNullOrEmpty(streetAddress))
                {
                    command.Parameters.AddWithValue("@raddress", streetAddress);
                }
                if (!String.IsNullOrEmpty(city))
                {
                    command.Parameters.AddWithValue("@city", city);
                }
                if (!String.IsNullOrEmpty(zip))
                {
                    command.Parameters.AddWithValue("@zip", zip);
                }

                Console.WriteLine("Querying Database for users...");
                SqlDataReader dataReader = await command.ExecuteReaderAsync();
                int count = 0;
                while (dataReader.Read())
                {
                    count++;
                    PersonModel temp = new PersonModel()
                    {
                        Id = Guid.NewGuid(),
                        lastname = dataReader.GetValue(0).ToString(),
                        firstname = dataReader.GetValue(1).ToString(),
                        dob = dataReader.GetValue(2).ToString(),
                        address = dataReader.GetValue(3).ToString(),
                        city = dataReader.GetValue(4).ToString(),
                        state = ApplicationConfiguration.DatabaseState
                    };

                    targets.Add(temp);
                }
                dataReader.Close();
                command.Dispose();
                _connection.Close();
                Console.WriteLine("ROWS: " + count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); /**nom nom nom  **/
            }

            return targets;
        }

    }
}
