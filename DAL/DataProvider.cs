using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuanLyCuaHangDienThoai.DAL
{
    public class DataProvider
    {
        // Thay đổi chuỗi kết nối cho phù hợp với SQL Server của bạn
        private string connectionString = @"Data Source=ADMIN-PC\SQLEXPRESS;Initial Catalog=QuanLyCuaHangDienThoai;Integrated Security=True";

        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set { instance = value; }
        }

        private DataProvider() { }

        /// <summary>
        /// Thực thi câu lệnh query và trả về một DataTable.
        /// </summary>
        /// <param name="query">Câu lệnh SQL cần thực thi.</param>
        /// <param name="parameter">Các tham số cho câu lệnh (tùy chọn).</param>
        /// <returns>DataTable chứa kết quả.</returns>
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    // Sử dụng Regex để tìm tất cả các tham số duy nhất trong câu lệnh
                    List<string> paramNames = new Regex(@"@\w+").Matches(query)
                                                                .Cast<Match>()
                                                                .Select(m => m.Value)
                                                                .Distinct()
                                                                .ToList();

                    if (paramNames.Count != parameter.Length)
                        throw new ArgumentException("Số lượng tham số truyền vào không khớp với số lượng tham số duy nhất trong câu lệnh.");

                    for (int i = 0; i < paramNames.Count; i++)
                    {
                        command.Parameters.AddWithValue(paramNames[i], parameter[i] ?? DBNull.Value);
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// Thực thi câu lệnh không trả về kết quả (INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="query">Câu lệnh SQL cần thực thi.</param>
        /// <param name="parameter">Các tham số cho câu lệnh (tùy chọn).</param>
        /// <returns>Số dòng bị ảnh hưởng.</returns>
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    // Sử dụng Regex để tìm tất cả các tham số trong câu lệnh
                    // Không dùng Distinct() vì một số câu lệnh (như INSERT) có thể có nhiều tham số
                    var parametersInQuery = new Regex(@"@\w+").Matches(query);
                    if (parametersInQuery.Count != parameter.Length)
                        throw new ArgumentException("Số lượng tham số truyền vào không khớp với số lượng tham số trong câu lệnh.");

                    for (int i = 0; i < parametersInQuery.Count; i++)
                    {
                        command.Parameters.AddWithValue(parametersInQuery[i].Value, parameter[i] ?? DBNull.Value);
                    }
                }

                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// Thực thi câu lệnh và trả về giá trị ở ô đầu tiên của dòng đầu tiên.
        /// </summary>
        /// <param name="query">Câu lệnh SQL cần thực thi.</param>
        /// <param name="parameter">Các tham số cho câu lệnh (tùy chọn).</param>
        /// <returns>Giá trị của ô đầu tiên.</returns>
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    // Sử dụng Regex để tìm tất cả các tham số duy nhất trong câu lệnh
                    List<string> paramNames = new Regex(@"@\w+").Matches(query)
                                                                .Cast<Match>()
                                                                .Select(m => m.Value)
                                                                .Distinct()
                                                                .ToList();

                    if (paramNames.Count != parameter.Length)
                        throw new ArgumentException("Số lượng tham số truyền vào không khớp với số lượng tham số duy nhất trong câu lệnh.");

                    for (int i = 0; i < paramNames.Count; i++)
                    {
                        command.Parameters.AddWithValue(paramNames[i], parameter[i] ?? DBNull.Value);
                    }
                }

                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }
    }
}