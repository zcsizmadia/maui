﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Maui.Handlers
{
	public partial class TimePickerHandler : ViewHandler<ITimePicker, MauiTimePicker>
	{
		protected override MauiTimePicker CreateNativeView()
		{
			return new MauiTimePicker(() =>
			{
				SetVirtualViewTime();
				NativeView?.ResignFirstResponder();
			});
		}

		protected override void ConnectHandler(MauiTimePicker nativeView)
		{
			if (nativeView != null)
				nativeView.ValueChanged += OnValueChanged;
		}

		protected override void DisconnectHandler(MauiTimePicker nativeView)
		{
			if (nativeView != null)
			{
				nativeView.RemoveFromSuperview();
				nativeView.ValueChanged -= OnValueChanged;
				nativeView.Dispose();
			}
		}

		public static void MapFormat(TimePickerHandler handler, ITimePicker timePicker)
		{
			handler.NativeView?.UpdateFormat(timePicker);
		}

		public static void MapTime(TimePickerHandler handler, ITimePicker timePicker)
		{
			handler.NativeView?.UpdateTime(timePicker);
		}

		public static void MapCharacterSpacing(TimePickerHandler handler, ITimePicker timePicker)
		{
			handler.NativeView?.UpdateCharacterSpacing(timePicker);
		}

		public static void MapFont(TimePickerHandler handler, ITimePicker timePicker)
		{
			_ = handler.Services ?? throw new InvalidOperationException($"{nameof(Services)} should have been set by base class.");

			var fontManager = handler.Services.GetRequiredService<IFontManager>();

			handler.NativeView?.UpdateFont(timePicker, fontManager);
		}

		void OnValueChanged(object? sender, EventArgs e)
		{
			SetVirtualViewTime();
		}

		void SetVirtualViewTime()
		{
			if (VirtualView == null || NativeView == null)
				return;

			VirtualView.Time = NativeView.Date.ToDateTime() - new DateTime(1, 1, 1);
		}
	}
}