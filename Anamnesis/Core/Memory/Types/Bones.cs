﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace Anamnesis.Memory
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Windows.Documents;
	using PropertyChanged;

	[StructLayout(LayoutKind.Explicit)]
	public struct Bones
	{
		[FieldOffset(0x10)]
		public int Count;

		[FieldOffset(0x18)]
		public IntPtr TransformArray;
	}

	public class BonesViewModel : MemoryViewModelBase<Bones>
	{
		public BonesViewModel(IntPtr pointer, IStructViewModel? parent = null)
			: base(pointer, parent)
		{
		}

		public BonesViewModel(IMemoryViewModel parent, string propertyName)
			: base(parent, propertyName)
		{
		}

		[ModelField] public int Count { get; set; }
		[ModelField] public IntPtr TransformArray { get; set; }

		public List<TransformViewModel> Transforms { get; set; } = new List<TransformViewModel>();

		protected override bool HandleModelToViewUpdate(PropertyInfo viewModelProperty, FieldInfo modelField)
		{
			bool changed = base.HandleModelToViewUpdate(viewModelProperty, modelField);

			this.Transforms.Clear();

			if (viewModelProperty.Name == nameof(BonesViewModel.TransformArray) && this.TransformArray != IntPtr.Zero)
			{
				IntPtr ptr = this.TransformArray + 0x10;
				for (int i = 0; i < this.Count; i++)
				{
					this.Transforms.Add(new TransformViewModel(ptr));
					ptr += 0x30;
				}
			}

			return changed;
		}
	}
}