using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Interface;
using TaskTracker.Models;

namespace TaskTracker.Domain
{
    public class DataAccess : IDataAccess
    {
        private readonly SqlConnection connection;
        public DataAccess(string con)
        {
            connection = new SqlConnection(con);
        }

        public List<CheckItems> GetTaskData(string user) 
        {
            List<CheckItems> items = new List<CheckItems>();

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = $@"select t.id,
                                            t.task,
                            		        case when cast(c.completed_date as date) = cast(getdate() AS date) then 1 else 0 end as completed
                                     from [TaskManagement].[dbo].[tasks] t
                                     left join [TaskManagement].[dbo].[tasksCompleted] c on c.task_id = t.id
                                                                                        and c.completedBy = @user";
                cmd.Parameters.Add("@user", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@user"].Value = user;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CheckItems model = new CheckItems()
                    {
                        id = Convert.ToInt32(dr["id"]),
                        thingToCheck = Convert.ToString(dr["task"]),
                        isCompleted = Convert.ToBoolean(dr["completed"])
                    };
                    items.Add(model);
                }
                return items;
            }
            catch (SqlException ex)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool CheckIfExists(int id, string user)
        {
            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = $@"select 1
                                     from [TaskManagement].[dbo].[tasksCompleted]
                                     where cast(getdate() as date) = cast(completed_date as date)
                                     and task_id = @task_id
                                     and completedBy = @user";
                cmd.Parameters.Add("@task_id", System.Data.SqlDbType.Int);
                cmd.Parameters.Add("@user", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@task_id"].Value = id;
                cmd.Parameters["@user"].Value = user;

                SqlDataReader dr = cmd.ExecuteReader();

                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                throw new Exception("CheckIfExist failed");
            }
            finally
            {
                connection.Close();
            }
        }

        public void Insert(int id, string user)
        {
            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = $@"insert into [TaskManagement].[dbo].[tasksCompleted] (task_id, completed_date, completedBy)
                                     values (@task_id, @completed_date, @completedBy)";
                cmd.Parameters.Add("@task_id", System.Data.SqlDbType.Int);
                cmd.Parameters.Add("@completed_date", System.Data.SqlDbType.DateTime);
                cmd.Parameters.Add("@completedBy", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@task_id"].Value = id;
                cmd.Parameters["@completed_date"].Value = DateTime.Now;
                cmd.Parameters["@completedBy"].Value = user;

                var count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    //worked
                }
            }
            catch (SqlException ex)
            {
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
