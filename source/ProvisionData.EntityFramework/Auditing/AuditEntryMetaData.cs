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
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.Json;

	internal class AuditEntryMetaData
	{
		public AuditEntryMetaData(EntityEntry entry)
		{
			Entry = entry;
		}

		public EntityEntry Entry { get; }
		public DateTime DateTime { get; set; } = DateTime.UtcNow;
		public String Username { get; set; }
		public AuditAction Action { get; set; }
		public String Entity { get; set; }
		public List<String> Changed { get; } = new List<String>();
		public Dictionary<String, Object> KeyValues { get; } = new Dictionary<String, Object>();
		public Dictionary<String, Object> Previous { get; } = new Dictionary<String, Object>();
		public Dictionary<String, Object> Current { get; } = new Dictionary<String, Object>();
		public Type Type { get; set; }
		public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
		public Boolean HasTemporaryProperties => TemporaryProperties.Any();

		public AuditEntry ToDto() => new()
		{
			DateTime = DateTime.SpecifyKind(DateTime, DateTimeKind.Utc),
			User = Username,
			Action = Action,
			Entity = Entity,
			Changed = Changed.Count == 0 ? null : JsonSerializer.Serialize(Changed),
			PrimaryKey = JsonSerializer.Serialize(KeyValues),
			Previous = Previous.Count == 0 ? null : JsonSerializer.Serialize(Previous),
			Current = Current.Count == 0 ? null : JsonSerializer.Serialize(Current),
			Type = $"{Type.FullName}, {Type.Assembly.GetName().Name}"
		};
	}
}
