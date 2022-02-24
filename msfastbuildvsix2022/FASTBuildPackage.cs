using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

using EnvDTE;
using EnvDTE80;

namespace msfastbuildvsix2022
{
	public class OptionPageGrid : DialogPage
	{
		private string FBArgs = "-dist -ide -j3";
		private string FBPath = "fbuild.exe";
		private bool FBUnity = false;
		private bool FBUseRelative = false;
		private bool FBUseLightCache = false;

		[Category("FASTBuild For Visual Studio")]
		[DisplayName("FASTBuild arguments")]
		[Description("Arguments that will be passed to FASTBuild, default \"-dist -ide -monitor\"")]
		public string OptionFBArgs
		{
			get { return FBArgs; }
			set { FBArgs = value; }
		}

		[Category("FASTBuild For Visual Studio")]
		[DisplayName("FBuild.exe path")]
		[Description("Can be used to specify the path to FBuild.exe")]
		public string OptionFBPath
		{
			get { return FBPath; }
			set { FBPath = value; }
		}

		[Category("FASTBuild For Visual Studio")]
		[DisplayName("Use unity files")]
		[Description("Whether to attempt to use 'unity' files to speed up compilation. May require modifying some headers.")]
		public bool OptionFBUnity
		{
			get { return FBUnity; }
			set { FBUnity = value; }
		}

		[Category("FASTBuild For Visual Studio")]
		[DisplayName("Use relative paths")]
		[Description("Whether to use relative paths.")]
		public bool OptionFBUseRelative
		{
			get { return FBUseRelative; }
			set { FBUseRelative = value; }
		}

		[Category("FASTBuild For Visual Studio")]
		[DisplayName("Use light cache")]
		[Description("Whether to use light cache.")]
		public bool OptionFBUseLightCache
		{
			get { return FBUseLightCache; }
			set { FBUseLightCache = value; }
		}
	}

	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(FASTBuildPackage.PackageGuidString)]
	[ProvideOptionPage(typeof(OptionPageGrid),
	"FASTBuild For Visual Studio", "Options", 0, 0, true)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	public sealed class FASTBuildPackage : Package
	{
		/// <summary>
		/// FASTBuildPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "18cf5831-f12a-49a6-ae26-7bdfa86755b7";

		public DTE2 m_dte;
		public OutputWindowPane m_outputPane;

		public FASTBuildPackage()
		{
		}

		public string OptionFBArgs
		{
			get
			{
				OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
				return page.OptionFBArgs;
			}
		}

		public string OptionFBPath
		{
			get
			{
				OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
				return page.OptionFBPath;
			}
		}

		public bool OptionFBUnity
		{
			get
			{
				OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
				return page.OptionFBUnity;
			}
		}

		public bool OptionFBUseRelative
		{
			get
			{
				OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
				return page.OptionFBUseRelative;
			}
		}

		public bool OptionFBUseLightCache
		{
			get
			{
				OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
				return page.OptionFBUseLightCache;
			}
		}

		protected override void Initialize()
		{
			FASTBuild.Initialize(this);
			base.Initialize();

			m_dte = (DTE2)GetService(typeof(DTE));
			OutputWindow outputWindow = m_dte.ToolWindows.OutputWindow;

			const string BUILD_OUTPUT_PANE_GUID = "{1BD8A850-02D1-11D1-BEE7-00A0C913D1F8}";
			foreach (OutputWindowPane pane in outputWindow.OutputWindowPanes)
			{
				if (pane.Guid.ToUpper() == BUILD_OUTPUT_PANE_GUID)
				{
					m_outputPane = pane;
					break;
				}
			}

			if (m_outputPane == null)
			{
				m_outputPane = outputWindow.OutputWindowPanes.Add("FASTBuild");
			}
			m_outputPane.OutputString("FASTBuild\r");
		}
	}
}
