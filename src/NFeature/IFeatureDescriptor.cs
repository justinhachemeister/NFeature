﻿// Copyright 2012, Ben Aston (ben@bj.ma).
// 
// This file is part of NFeature.
// 
// NFeature is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NFeature is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NFeature.  If not, see <http://www.gnu.org/licenses/>.

namespace NFeature
{
	using System.Collections.Generic;

	/// <summary>
	/// 	A description of the state of a feature. Metadata for members of the Feature enumeration.
	/// </summary>
	public interface IFeatureDescriptor<TFeatureEnum>
	{
		/// <summary>
		/// 	Calculated by the manifest creation strategy. Might be based on feature dependencies and cookies, for example.
		/// </summary>
		bool IsAvailable { get; set; }

		IList<TFeatureEnum> Dependencies { get; set; }

		IDictionary<string, dynamic> Settings { get; set; }
	}
}