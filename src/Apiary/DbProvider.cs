﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IT;
using Apiary.Data;
using Apiary.M;

namespace Apiary
{
	class DbProvider : ILog
	{
		public static readonly DbProvider Instance = new DbProvider();

		private DB db;

		private DbProvider()
		{
			this.db = new DB();
		}


		#region dictionary

		public IEnumerable<M_Beehive> List_Beehive()
		{
			this.Debug("()");
			try
			{
				return M_Family.Beehives = this.db.S_Dictionary.R_Beehive.List(false)
					.ToModel()
					.ToArray()
					;
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public void Set_Beehive(IEnumerable<IM_Beehive> value)
		{
			this.Debug("()");
			try
			{
				var i = this.db.S_Dictionary.R_Beehive.Set(value);
				M_Family.Beehives = value.ToModel();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}


		public IEnumerable<IM_Operation> List_Operation()
		{
			this.Debug("()");
			try
			{
				return this.db.S_Dictionary.R_FamilyOperation.List();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public void Set_Operation(IEnumerable<IM_Operation> value)
		{
			this.Debug("()");
			try
			{
				var i = this.db.S_Dictionary.R_FamilyOperation.Set(value);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}


		public IEnumerable<IM_FamilyProperty> List_FamilyProperty()
		{
			this.Debug("()");
			try
			{
				return this.db.S_Dictionary.R_FamilyProperty.List();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public void Set_FamilyProperty(IEnumerable<IM_FamilyProperty> value)
		{
			this.Debug("()");
			try
			{
				var i = this.db.S_Dictionary.R_FamilyProperty.Set(value);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		#endregion

		#region family

		public M.M_Family Get_Family(long id)
		{
			this.Debug("()");
			try
			{
				return this.db.S_Family.R_Family.Get(id).ToModel();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
			return null;
		}

		public IM_FamilyInfo Get_FamilyInfo(IM_Family family)
		{
			this.Debug("()");
			try
			{
				return this.db.S_Family.Get_FamilyInfo(family);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
			return null;
		}


		public IEnumerable<M.M_Family> List_Family(bool withHidden = false)
		{
			this.Debug("()");
			try
			{
				var res = this.db.S_Family.R_Family.List(withHidden);
				return res.ToModel();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public IEnumerable<M.M_Family> List_Family_ByBeehive(IM_Beehive beehive)
		{
			this.Debug("()");
			try
			{
				if (beehive != null)
				{
					var res = this.db.S_Family.R_Family.Get_ByBeehive(beehive);
					return res.ToModel();
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
			return null;
		}

		public IEnumerable<IM_FamilyInfo> List_FamilyInfo_ByBeehive(IM_Beehive beehive)
		{
			this.Debug("()");
			try
			{
				if (beehive != null)
					return this.db.S_Family.Get_ByBeehive(beehive);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
			return null;
		}


		public int Set_Family(IM_Family value)
		{
			this.Debug("()");
			try
			{
				return this.db.S_Family.R_Family.Set(value);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		#endregion

	}
}
