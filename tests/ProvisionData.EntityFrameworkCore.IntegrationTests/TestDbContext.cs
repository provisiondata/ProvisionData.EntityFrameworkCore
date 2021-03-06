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
	using Bogus;
	using Microsoft.EntityFrameworkCore;
	using ProvisionData.EntityFrameworkCore.Auditing;
	using System;

	public sealed class TestDbContext : AuditedDbContext
	{
		public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) {
		}

		public DbSet<FamilyMember> Members { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Invoice>  Invoices { get; set; }
		public DbSet<LineItem> LineItems { get; set; }
		public DbSet<Product> Products { get; set; }

		public override String GetUsername() => new Faker().Person.FullName;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CustomerConfiguration());
			modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
			modelBuilder.ApplyConfiguration(new LineItemConfiguration());
			modelBuilder.ApplyConfiguration(new ProductConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
