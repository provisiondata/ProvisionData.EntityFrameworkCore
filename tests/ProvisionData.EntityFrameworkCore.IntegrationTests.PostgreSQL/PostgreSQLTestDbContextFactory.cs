/*******************************************************************************
* MIT License
*
* Copyright 2021 Provision Data Systems Inc.  https://provisiondata.com
*
* Permission is hereby granted, free of charge, to any person obtaining a
* copy of this software and associated documentation files (the "Software"),
* to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sub-license,
* and/or sell copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*
*******************************************************************************/

namespace ProvisionData.EntityFrameworkCore
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Design;
	using Npgsql;
	using System;
	using System.Data.Common;

	public class PostgreSQLTestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
	{
		// docker run --name integration-testing-postgres -e POSTGRES_PASSWORD=password -p 5432:5432/tcp -d postgres
		public const String ConnectionString = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=password;";

		public static DbContextOptions<TestDbContext> GetOptions()
		{
			var builder = new DbContextOptionsBuilder<TestDbContext>();

			builder.UseNpgsql(GetDbConnection(), options => options.MigrationsAssembly(typeof(PostgreSQLTestDbContextFactory).AssemblyQualifiedName))
				   .EnableSensitiveDataLogging(true);

			return builder.Options;
		}

		public TestDbContext CreateDbContext(String[] args) => new(GetOptions());

		public static DbConnection GetDbConnection() => new NpgsqlConnection(ConnectionString);
	}
}
