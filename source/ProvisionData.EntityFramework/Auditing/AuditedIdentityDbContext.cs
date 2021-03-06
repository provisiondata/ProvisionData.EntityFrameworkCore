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

namespace ProvisionData.EntityFrameworkCore.Auditing
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public abstract class AuditedIdentityDbContext<TUser> : IdentityDbContext<TUser>, IAuditedDbContext
		where TUser : IdentityUser
	{
		public AuditedIdentityDbContext(DbContextOptions options) : base(options) { }

		public DbSet<AuditEntry> AuditLogs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
			=> base.OnModelCreating(modelBuilder.ConfigureAuditLog());

		[SuppressMessage("Usage", "VSTHRD103:Call async methods when in an async method", Justification = "Only need to use the async method when an async value generator is used.")]
		public override async Task<Int32> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = this.GetAuditEntries(GetUsername());
			AuditLogs.AddRange(entries.Normal());
			var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
			if (entries.Temporary().Any())
			{
				AuditLogs.AddRange(entries.Temporary());
				await base.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
			}
			return result;
		}

		public virtual String GetUsername() => String.Empty;
	}
}
