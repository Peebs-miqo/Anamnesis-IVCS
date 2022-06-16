﻿// © Anamnesis.
// Licensed under the MIT license.

namespace Anamnesis.Panels.Navigation;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using XivToolsWpf.DependencyProperties;

/// <summary>
/// Interaction logic for ActorEntry.xaml.
/// </summary>
public partial class ActorEntry : UserControl
{
	public static readonly IBind<bool> IsExpandedDp = Binder.Register<bool, ActorEntry>(nameof(IsExpanded), OnIsExpandedChanged);
	public static readonly IBind<bool> ShowTextDp = Binder.Register<bool, ActorEntry>(nameof(ShowText));

	public static readonly RoutedEvent? CollapsedEvent;
	public static readonly RoutedEvent? ExpandedEvent;

	public ActorEntry()
	{
		this.InitializeComponent();
		this.ContentArea.DataContext = this;

		this.ContentScale.ScaleY = this.IsExpanded ? 1 : 0;
	}

	public event RoutedEventHandler? Collapsed;
	public event RoutedEventHandler? Expanded;

	public PinnedActor? Actor => this.DataContext as PinnedActor;

	public bool IsExpanded
	{
		get => IsExpandedDp.Get(this);
		set => IsExpandedDp.Set(this, value);
	}

	public bool ShowText
	{
		get => ShowTextDp.Get(this);
		set => ShowTextDp.Set(this, value);
	}

	private static void OnIsExpandedChanged(ActorEntry sender, bool value)
	{
		if (value)
		{
			sender.Expanded?.Invoke(sender, new());
			Storyboard? sb = sender.ExpanderContent.Resources["ExpandStoryboard"] as Storyboard;
			sb?.Begin();
		}
		else
		{
			sender.Collapsed?.Invoke(sender, new());
			Storyboard? sb = sender.ExpanderContent.Resources["CollapseStoryboard"] as Storyboard;
			sb?.Begin();
		}
	}

	private void OnUnpinActorClicked(object sender, RoutedEventArgs e)
	{
		if (sender is FrameworkElement el && el.DataContext is PinnedActor actor)
		{
			TargetService.UnpinActor(actor);
		}
	}

	private void OnTargetActorClicked(object sender, RoutedEventArgs e)
	{
		if (sender is FrameworkElement el && el.DataContext is PinnedActor actor)
		{
			TargetService.SetPlayerTarget(actor);
		}
	}

	private void OnActorPinPreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		if (e.ChangedButton == MouseButton.Middle)
		{
			this.OnUnpinActorClicked(sender, new RoutedEventArgs());
		}
	}
}
