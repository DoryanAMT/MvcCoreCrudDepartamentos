using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using MvcCoreCrudDepartamentos.Models;

namespace MvcCoreCrudDepartamentos.Repositories
{
    #region procedimientos almacenados
    //create or alter procedure SP_INSERT_DEPARTAMENT
    //(@nombre nvarchar(50), @localidad nvarchar(50))
    //as
	//declare @nextId int
    //select @nextId = MAX(DEPT_NO) + 1 FROM DEPT
    //insert into DEPT values
    //(@nextId, @nombre, @localidad)
    //go
    #endregion
    public class RepositoryDepartamentos
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        public RepositoryDepartamentos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            string sql = "select * from DEPT";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            List<Departamento> departamentos = new List<Departamento>();
            while (await this.reader.ReadAsync())
            {
                Departamento departamento = new Departamento();
                departamento.IdDepartamento = int.Parse(this.reader["DEPT_NO"].ToString());
                departamento.Nombre = this.reader["DNOMBRE"].ToString();
                departamento.Localidad = this.reader["LOC"].ToString();
                departamentos.Add(departamento);
            }
            await this.cn.CloseAsync();
            await this.reader.CloseAsync();
            return departamentos;
        }
        public async Task InsertDepartamentoAsync
            (string nombre, string localidad)
        {
            string sql = "SP_INSERT_DEPARTAMENT";
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@localidad", localidad);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
        public async Task<Departamento> FindDepartamentoAsync
            (int idDepartamento)
        {
            string sql = "select * from DEPT where DEPT_NO=@iddepartamento";
            this.com.Parameters.AddWithValue("@iddepartamento", idDepartamento);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            Departamento departamento = new Departamento();
            await this.reader.ReadAsync();
            departamento.IdDepartamento = int.Parse(this.reader["DEPT_NO"].ToString());
            departamento.Nombre = this.reader["DNOMBRE"].ToString();
            departamento.Localidad = this.reader["LOC"].ToString();
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return departamento;
        }
        public async Task UpdateDepartamentoAsync
            (int idDepartamento, string nombre, string localidad)
        {
            string sql = "update DEPT set DNOMBRE=@nombre," +
                "LOC=@localidad " +
                "where DEPT_NO=@iddepartamento";
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@localidad", localidad);
            this.com.Parameters.AddWithValue("@iddepartamento", idDepartamento);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
        public async Task DeleteDepartamentoAsync
            (int idDepartamento)
        {
            string sql = "delete from DEPT where DEPT_NO=@iddepartamento";
            this.com.Parameters.AddWithValue("@iddepartamento", idDepartamento);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            this.com.Parameters.Clear();
        }
    }
}
