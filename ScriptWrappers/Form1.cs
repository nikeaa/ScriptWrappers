using PsDotNet;
using PsDotNet.Collections;
using PsDotNet.Colors;
using PsDotNet.LayerStyles;
using PsDotNet.Options.Save;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ScriptWrappers
{
	public partial class Form1 : Form
	{
		//private IPsApplication connection = PsConnection.StartAndConnect();
		private IPsApplication app = PsConnection.Application;

		private readonly int HEIGHT = 1575;
		private readonly int WIDTH = 1575;

		//private Array COMPANY_NAME_TEXT_VERTICAL_POSITION = new Array[10, 600];
		//private Array PRODUCT_CODE_TEXT_VERTICAL_POSITION = new Array[10, 1450];
		//private Array COMPANY_NAME_TEXT_HORIZONTAL_POSITION = new Array[650, 1640];
		//private Array PRODUCT_CODE_TEXT_HORIZONTAL_POSITION = new Array[1450, 1640];

		private int COMPANY_NAME_FONT_SIZE = 6;
		private int PRODUCT_CODE_FONT_SIZE = 8;
		private int JPEG_QUALITY = 8;

		private int TWO_UP_DOC_WIDTH = 3300;
		private int TWO_UP_DOC_HEIGHT = 2550;
		private int TWO_UP_DOC_RESOLUTION = 300;

		private int WEB_DOC_WIDTH = 2400;
		private int WEB_DOC_HEIGHT = 1950;
		private int WEB_DOC_RESOLUTION = 300;


		private int VERTICAL_FRONT_X = 665;
		private int VERTICAL_FRONT_Y = 0;
		private int VERTICAL_FRONT_WIDTH = 644;
		private int VERTICAL_FRONT_HEIGHT = 1575;

		private int VERTICAL_BACK_TOP_X = 1309;
		private int VERTICAL_BACK_TOP_Y = 0;
		private int VERTICAL_BACK_TOP_WIDTH = 266; // 378
		private int VERTICAL_BACK_TOP_HEIGHT = 1575;

		private int VERTICAL_BACK_BOTTOM_X = 266;
		private int VERTICAL_BACK_BOTTOM_Y = 0;
		private int VERTICAL_BACK_BOTTOM_WIDTH = 644 - 266; //415;
		private int VERTICAL_BACK_BOTTOM_HEIGHT = 1575;

		private int VERTICAL_TRANSLATE_X = -402;
		private int VERTICAL_TRANSLATE_FRONT_Y = -2;
		private int VERTICAL_TRANSLATE_BACK_BOTTOM_Y = 255 + 317;
		private int VERTICAL_TRANSLATE_BACK_TOP_Y = 265;


		private int HORIZONTAL_FRONT_X = 0;
		private int HORIZONTAL_FRONT_Y = 665;
		private int HORIZONTAL_FRONT_WIDTH = 1575;
		private int HORIZONTAL_FRONT_HEIGHT = 644;

		private int HORIZONTAL_BACK_TOP_X = 0;
		private int HORIZONTAL_BACK_TOP_Y = 1309;
		private int HORIZONTAL_BACK_TOP_WIDTH = 1575;
		private int HORIZONTAL_BACK_TOP_HEIGHT = 266;

		private int HORIZONTAL_BACK_BOTTOM_X = 0;
		private int HORIZONTAL_BACK_BOTTOM_Y = 266;
		private int HORIZONTAL_BACK_BOTTOM_WIDTH = 1575;
		private int HORIZONTAL_BACK_BOTTOM_HEIGHT = 644 - 266;

		private int HORIZONTAL_TRANSLATE_X = -2;
		private int HORIZONTAL_TRANSLATE_FRONT_Y = -290;
		private int HORIZONTAL_TRANSLATE_BACK_BOTTOM_Y = 253 + 317;
		private int HORIZONTAL_TRANSLATE_BACK_TOP_Y = 253;

		private string CurrentVersion
		{
			get
			{
				return ApplicationDeployment.IsNetworkDeployed
					   ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
					   : Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public Form1()
		{
			InitializeComponent();

			lblVersion.Text = "v" + CurrentVersion;

			string directory = "D:\\Google Drive\\Nikeaa Design LLC\\Wrappers\\ZSW - Software\\ScriptWrappers";
			string licensePath = Path.Combine(directory, "PsDotNet_Library.lic");
			RegistrationResult registerResult = PsConnection.RegisterLicense(licensePath);

			this.AllowDrop = true;
			this.DragEnter += new DragEventHandler(Form1_DragEnter);
			this.DragDrop += new DragEventHandler(Form1_DragDrop);

			app.Preferences.RulerUnits = EPsUnits.psPixels;
			app.Preferences.TypeUnits = EPsTypeUnits.psTypePixels;
			app.DisplayDialogs = EPsDialogModes.psDisplayNoDialogs;
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string fileName in fileNames)
			{
				FileAttributes attributes = File.GetAttributes(fileName);
				if (attributes.HasFlag(FileAttributes.Directory))
				{
					List<string> items = GetFilesInDirectory(fileName);
					foreach (string item in items)
					{
						FilesToConvertListBox.Items.Add(item);
					}
				}
				else
				{
					if (Path.GetExtension(fileName) == ".psd")
					{
						FilesToConvertListBox.Items.Add(fileName);
					}
				}

				ConvertButton.Enabled = (FilesToConvertListBox.Items.Count > 0);
			}
		}

		private void BrowseButton_Click(object sender, EventArgs e)
		{
			if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				TemplatePathTextBox.Text = FolderBrowserDialog.SelectedPath;
			}
		}

		private void ConvertButton_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrWhiteSpace(TemplatePathTextBox.Text))
			{
				if (FilesToConvertListBox.Items.Count > 0)
				{
					string emblem = GetEmblem();

					ConvertButton.Enabled = false;

					CloseOpenDocuments();

					for (int i = FilesToConvertListBox.Items.Count - 1; i >= 0; i--)
					{
						string fileToProcess = FilesToConvertListBox.Items[i].ToString();

						try
						{
							ScriptWrapper(fileToProcess, emblem);
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message + ", line:" + ex.StackTrace, "Error Processing File: " + fileToProcess);
						}

						FilesToConvertListBox.Items.RemoveAt(i);
					}
				}
				else
				{
					MessageBox.Show("Please select at least one file to convert.");
				}
			}
			else
			{
				MessageBox.Show("Please select a template directory.");
			}
		}

		private string GetEmblem()
		{
			string result = "None";

			foreach (Control control in gbEmblem.Controls)
			{
				RadioButton radio = control as RadioButton;
				if (radio.Checked)
				{
					result = radio.Text;
					break;
				}
			}

			return result;
		}

		private List<string> GetFilesInDirectory(string directoryName)
		{
			List<string> result = new List<string>();

			foreach (string fileName in Directory.GetFiles(directoryName))
			{
				if (Path.GetExtension(fileName) == ".psd")
				{
					result.Add(fileName);
				}
			}

			foreach (string dirName in Directory.GetDirectories(directoryName))
			{
				result.AddRange(GetFilesInDirectory(dirName));
			}

			return result;
		}

		private void ScriptWrapper(string fileName, string emblem)
		{
			IPsJPEGSaveOptions jpegOptions;
			string jpegFileName;
			IPsDocument twoUpDoc, jpegDoc;
			IPsDocument webDoc, webTemplateDoc;

			IPsDocument doc = app.Open(fileName);

			String path = Path.GetDirectoryName(fileName);
			String psdFileName = Path.GetFileName(fileName);

			String productCode = psdFileName.Replace(".psd", "");

			bool isVertical = productCode.Last() == 'V';

			UpdateOriginalDocument(doc, path, productCode, isVertical, out jpegOptions, out jpegFileName);
			CreateTwoUpVersion(doc, path, productCode, jpegOptions, jpegFileName, out twoUpDoc, out jpegDoc);
			CreateWebVersion(path, productCode, isVertical, jpegOptions, jpegDoc, emblem, out webDoc, out webTemplateDoc);

			// Close the open documents
			webDoc.Close(EPsSaveOptions.psDoNotSaveChanges);
			webTemplateDoc.Close(EPsSaveOptions.psDoNotSaveChanges);
			twoUpDoc.Close(EPsSaveOptions.psDoNotSaveChanges);
			jpegDoc.Close(EPsSaveOptions.psDoNotSaveChanges);
			doc.Close(EPsSaveOptions.psDoNotSaveChanges);
		}

		private void UpdateOriginalDocument(IPsDocument doc, string path, string productCode, bool isVertical, out IPsJPEGSaveOptions jpegOptions, out string jpegFileName)
		{
			AddCompanyName(doc, isVertical);
			AddProductCode(doc, productCode, isVertical);

			// Save the PSD document.
			doc.Save();

			// Save as a JPEG.
			jpegOptions = app.CreateJPEGSaveOptions();
			jpegOptions.Quality = JPEG_QUALITY;
			jpegOptions.EmbedColorProfile = true;
			jpegOptions.FormatOptions = EPsFormatOptionsType.psStandardBaseline;
			jpegOptions.Matte = EPsMatteType.psNoMatte;

			jpegFileName = path + "\\" + productCode + ".jpg";
			doc.SaveAs(jpegFileName, jpegOptions, false);
		}

		private void CreateWebVersion(string path, string productCode, bool isVertical, IPsJPEGSaveOptions jpegOptions, IPsDocument jpegDoc, string emblem, out IPsDocument webDoc, out IPsDocument webTemplateDoc)
		{
			// Create the web document.
			webDoc = app.Documents.Add(WEB_DOC_WIDTH, WEB_DOC_HEIGHT, WEB_DOC_RESOLUTION, "Web Doc", EPsDocumentMode.psRGB);
			//app.ActiveDocument = webDoc;

			// Get the web template background.
			string webTemplateFileName = getWebTemplateFileName(TemplatePathTextBox.Text, productCode);
			webTemplateDoc = app.Open(TemplatePathTextBox.Text + "\\" + webTemplateFileName);
			app.ActiveDocument = webTemplateDoc;

			IPsDocument logoBadgeDoc = app.Open(TemplatePathTextBox.Text + "\\Logo Badge.psd");
			logoBadgeDoc.Selection.SelectAll();
			logoBadgeDoc.Selection.Copy(true);
			app.ActiveDocument = webTemplateDoc;
			webTemplateDoc.Paste(false);
			webTemplateDoc.ActiveLayer.Translate(-940, -690);
			logoBadgeDoc.Close(EPsSaveOptions.psDoNotSaveChanges);

			if (emblem == "No Editable") {
				IPsDocument optionsBadgeDoc = app.Open(TemplatePathTextBox.Text + "\\Options Badge.psd");
				optionsBadgeDoc.Selection.SelectAll();
				optionsBadgeDoc.Selection.Copy(true);
				app.ActiveDocument = webTemplateDoc;
				webTemplateDoc.Paste(false);
				webTemplateDoc.ActiveLayer.Translate(-940, 670);
				optionsBadgeDoc.Close(EPsSaveOptions.psDoNotSaveChanges);
			}

			//IPsArtLayer editableLayer = webTemplateDoc.ArtLayers.GetAllByName("editable").FirstOrDefault();
			//IPsArtLayer noEditableLayer = webTemplateDoc.ArtLayers.GetAllByName("No editable").FirstOrDefault();
			//switch (emblem)
			//{
			//	case "None":
			//		if (editableLayer != null) editableLayer.Visible = false;
			//		if (noEditableLayer != null) noEditableLayer.Visible = false;
			//		break;
			//	case "No Editable":
			//		if (editableLayer != null) editableLayer.Visible = false;
			//		break;
			//	case "With Editable":
			//		if (noEditableLayer != null) noEditableLayer.Visible = false;
			//		break;
			//}

			//webTemplateDoc.MergeVisibleLayers();
			app.ActiveDocument = webTemplateDoc;
			webTemplateDoc.ResizeImage(WEB_DOC_WIDTH, WEB_DOC_HEIGHT, EPsResampleMethod.psAutomatic);
			webTemplateDoc.Selection.SelectAll();
			webTemplateDoc.Selection.Copy(true);
			app.ActiveDocument = webDoc;
			webDoc.Paste(false);

			if (isVertical)
			{
				webDoc = addWrapperToVerticalTemplate(webTemplateDoc, jpegDoc, webDoc);
			}
			else
			{
				webDoc = addWrapperToHorizontalTemplate(webTemplateDoc, jpegDoc, webDoc);
			}

			// Save the web JPEG.
			string webFileName = path + "\\" + productCode + "_web.jpg";
			webDoc.SaveAs(webFileName, jpegOptions, false);
		}

		private void AddProductCode(IPsDocument doc, string productCode, bool isVertical)
		{
			// Select and delete existing product code layers.
			try
			{
				var layers = doc.ArtLayers.GetAllByName("Product Code");
				foreach (IPsArtLayer layer in layers)
				{
					layer.Delete();
				}
			}
			catch { }

			// Add the product code text.
			IPsArtLayer productCodeText = doc.ArtLayers.AddTextLayer("Product Code");
			productCodeText.TextItem.Contents = productCode;
			productCodeText.TextItem.Size = PRODUCT_CODE_FONT_SIZE;
			productCodeText.TextItem.FauxBold = true;

			// Move the company name and product code to the proper positions
			// depending on if the wrapper is a horizontal or vertical wrapper.
			if (isVertical)
			{
				productCodeText.Rotate(-90, EPsAnchorPosition.psMiddleCenter);
				productCodeText.TextItem.Position = new PointF(WIDTH - 20, 200);
			}
			else
			{
				productCodeText.TextItem.Position = new PointF(WIDTH - 200, HEIGHT - 20);
			}
		}

		private void AddCompanyName(IPsDocument doc, bool isVertical)
		{
			// Select and delete existing company name/number layers.
			try
			{
				var layers = doc.ArtLayers.GetAllByName("Company Name/Number");
				foreach (IPsArtLayer layer in layers)
				{
					layer.Delete();
				}
			}
			catch { }

			// Add the company name text.
			IPsArtLayer companyNameText = doc.ArtLayers.AddTextLayer("Company Name/Number");
			companyNameText.TextItem.Contents = "Nikeaa Design, LLC - 319-538-1167";
			companyNameText.TextItem.Size = COMPANY_NAME_FONT_SIZE;
			if (isVertical)
			{
				companyNameText.Rotate(-90, EPsAnchorPosition.psMiddleCenter);
				companyNameText.TextItem.Position = new PointF(WIDTH - 20, (HEIGHT + 400) / 2);
			}
			else
			{
				companyNameText.TextItem.Position = new PointF((WIDTH - 400) / 2, HEIGHT - 20);
			}
		}

		private void CreateTwoUpVersion(IPsDocument doc, string path, string productCode, IPsJPEGSaveOptions jpegOptions, string jpegFileName, out IPsDocument twoUpDoc, out IPsDocument jpegDoc)
		{
			// Create a document for the two up version.
			twoUpDoc = app.Documents.Add(TWO_UP_DOC_WIDTH, TWO_UP_DOC_HEIGHT, TWO_UP_DOC_RESOLUTION, "Two Up", EPsDocumentMode.psRGB);

			app.ActiveDocument = twoUpDoc;

			// Open up the jpg wrapper file.
			jpegDoc = app.Open(jpegFileName);
			app.ActiveDocument = jpegDoc;
			jpegDoc.Selection.SelectAll();
			jpegDoc.Selection.Copy(false);
			app.ActiveDocument = twoUpDoc;
			IPsArtLayer left = twoUpDoc.ArtLayers.AddNormalLayer("Left");
			twoUpDoc.Paste(false);
			IPsArtLayer right = twoUpDoc.ArtLayers.AddNormalLayer("Right");
			twoUpDoc.Paste(false);

			// Move them to the correct positions.
			left.Translate(-(WIDTH / 2), 0);
			right.Translate(+(WIDTH / 2), 0);

			// Draw vertical cut lines.
			DrawCutLines(twoUpDoc, jpegDoc);

			// Save the two up jpeg.
			string twoUpFileName = path + "\\" + productCode + "_2Up.jpg";
			doc.SaveAs(twoUpFileName, jpegOptions, false);
		}

		private void DrawCutLines(IPsDocument twoUpDoc, IPsDocument jpegDoc)
		{
			int leftVerticalCut = (int)((twoUpDoc.Width - jpegDoc.Width * 2) / 2);
			int middleVerticalCut = (int)(leftVerticalCut + jpegDoc.Width);
			int rightVerticalCut = (int)(middleVerticalCut + jpegDoc.Width);
			int topVerticalCut = (int)((twoUpDoc.Height - jpegDoc.Height) / 2);
			int bottomVerticalCut = (int)(topVerticalCut + jpegDoc.Height);
			int topHorizontalCut = topVerticalCut;
			int bottomHorizontalCut = bottomVerticalCut;

			DrawLine(twoUpDoc, leftVerticalCut, 0, leftVerticalCut, topVerticalCut, "Crop 1");
			DrawLine(twoUpDoc, leftVerticalCut, bottomVerticalCut, leftVerticalCut, (int)twoUpDoc.Height, "Crop 2");
			DrawLine(twoUpDoc, middleVerticalCut, 0, middleVerticalCut, topVerticalCut, "Crop 3");
			DrawLine(twoUpDoc, middleVerticalCut, bottomVerticalCut, middleVerticalCut, (int)twoUpDoc.Height, "Crop 4");
			DrawLine(twoUpDoc, rightVerticalCut, 0, rightVerticalCut, topVerticalCut, "Crop 5");
			DrawLine(twoUpDoc, rightVerticalCut, bottomVerticalCut, rightVerticalCut, (int)twoUpDoc.Height, "Crop 6");

			// Draw horizonal cut lines.
			DrawLine(twoUpDoc, 0, topHorizontalCut, 70, topHorizontalCut, "Crop 7");
			DrawLine(twoUpDoc, (int)twoUpDoc.Width - 70, topHorizontalCut, (int)twoUpDoc.Width, topHorizontalCut, "Crop 8");
			DrawLine(twoUpDoc, 0, bottomHorizontalCut, 70, bottomHorizontalCut, "Crop 9");
			DrawLine(twoUpDoc, (int)twoUpDoc.Width - 70, bottomHorizontalCut, (int)twoUpDoc.Width, bottomHorizontalCut, "Crop 10");
		}

		private IPsDocument addWrapperToHorizontalTemplate(IPsDocument webTemplateDoc, IPsDocument jpegDoc, IPsDocument webDoc)
		{
			webTemplateDoc.RotateCanvas(90);

			app.ActiveDocument = webDoc;

			// Build the front of the wrapper, with ends.
			IPsArtLayer frontLeftEndLayer = webDoc.ArtLayers.AddNormalLayer("Front Left End");
			DrawFilledRectangle(webDoc, 360, 363, 410, 1007, "Front Left End");

			IPsArtLayer frontRightEndLayer = webDoc.ArtLayers.AddNormalLayer("Front Right End");
			DrawFilledRectangle(webDoc, 1985, 363, 2035, 1007, "Front Right End");

			IPsArtLayer frontLayer = webDoc.ArtLayers.AddNormalLayer("Front");
			IPsArtLayer front = AddJpegAreaToWebDoc(jpegDoc, webDoc, HORIZONTAL_FRONT_X, HORIZONTAL_FRONT_Y, HORIZONTAL_FRONT_WIDTH, HORIZONTAL_FRONT_HEIGHT, HORIZONTAL_TRANSLATE_X, HORIZONTAL_TRANSLATE_FRONT_Y, true);

			List<IPsLayer> layersToMerge = new List<IPsLayer>();
			layersToMerge.Add(frontLeftEndLayer);
			layersToMerge.Add(front);
			IPsArtLayer frontMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			layersToMerge.Clear();
			layersToMerge.Add(frontRightEndLayer);
			layersToMerge.Add(frontMerged);
			frontMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			frontMerged.Name = "Front";

			// Build the back of the wrapper, with ends.
			IPsArtLayer back1 = AddJpegAreaToWebDoc(jpegDoc, webDoc, HORIZONTAL_BACK_BOTTOM_X, HORIZONTAL_BACK_BOTTOM_Y, HORIZONTAL_BACK_BOTTOM_WIDTH, HORIZONTAL_BACK_BOTTOM_HEIGHT, HORIZONTAL_TRANSLATE_X, HORIZONTAL_TRANSLATE_BACK_BOTTOM_Y, false);
			IPsArtLayer back2 = AddJpegAreaToWebDoc(jpegDoc, webDoc, HORIZONTAL_BACK_TOP_X, HORIZONTAL_BACK_TOP_Y, HORIZONTAL_BACK_TOP_WIDTH, HORIZONTAL_BACK_TOP_HEIGHT, HORIZONTAL_TRANSLATE_X, HORIZONTAL_TRANSLATE_BACK_TOP_Y, false);

			// Merge the two back pieces.
			layersToMerge.Clear();
			layersToMerge.Add(back1);
			layersToMerge.Add(back2);
			IPsArtLayer backMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			backMerged.Name = "Back";

			IPsLayerStyle layerStyle = app.CreateLayerStyle();
			IPsBevelEmbossStyle bevelStyle = layerStyle.AddBevelEmbossStyle();
			bevelStyle.Size = 20;
			backMerged.ApplyLayerStyle(layerStyle);

			IPsArtLayer backLeftEndLayer = webDoc.ArtLayers.AddNormalLayer("Back Left End");
			DrawFilledRectangle(webDoc, 360, 1094, 410, 1738, "Back Left End");

			IPsArtLayer backRightEndLayer = webDoc.ArtLayers.AddNormalLayer("Back Right End");
			DrawFilledRectangle(webDoc, 1985, 1094, 2035, 1738, "Back Right End");

			layersToMerge.Clear();
			layersToMerge.Add(backLeftEndLayer);
			layersToMerge.Add(backMerged);
			backMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			layersToMerge.Clear();
			layersToMerge.Add(backRightEndLayer);
			layersToMerge.Add(backMerged);
			backMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			backMerged.Name = "Back";

			backMerged.Rotate(15.0, EPsAnchorPosition.psBottomRight);

			backMerged.MoveAfter(frontMerged);
			backMerged.Translate(-140, 0);

			return webDoc;
		}

		private IPsDocument addWrapperToVerticalTemplate(IPsDocument webTemplateDoc, IPsDocument jpegDoc, IPsDocument webDoc)
		{
			webTemplateDoc.RotateCanvas(90);

			app.ActiveDocument = webDoc;

			// Build the front of the wrapper, with ends.
			IPsArtLayer frontLayer = webDoc.ArtLayers.AddNormalLayer("Front");
			IPsArtLayer front = AddJpegAreaToWebDoc(jpegDoc, webDoc, VERTICAL_FRONT_X, VERTICAL_FRONT_Y, VERTICAL_FRONT_WIDTH, VERTICAL_FRONT_HEIGHT, VERTICAL_TRANSLATE_X, VERTICAL_TRANSLATE_FRONT_Y, true);

			IPsArtLayer frontLeftEndLayer = webDoc.ArtLayers.AddNormalLayer("Front Left End");
			DrawFilledRectangle(webDoc, 471, 135, 1121, 185, "Front Left End");

			IPsArtLayer frontRightEndLayer = webDoc.ArtLayers.AddNormalLayer("Front Right End");
			DrawFilledRectangle(webDoc, 471, 1750, 1121, 1800, "Front Right End");

			List<IPsLayer> layersToMerge = new List<IPsLayer>();
			layersToMerge.Add(frontLeftEndLayer);
			layersToMerge.Add(front);
			IPsArtLayer frontMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			layersToMerge.Clear();
			layersToMerge.Add(frontRightEndLayer);
			layersToMerge.Add(frontMerged);
			frontMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			frontMerged.Name = "Front";

			IPsArtLayer back1 = AddJpegAreaToWebDoc(jpegDoc, webDoc, VERTICAL_BACK_BOTTOM_X, VERTICAL_BACK_BOTTOM_Y, VERTICAL_BACK_BOTTOM_WIDTH, VERTICAL_BACK_BOTTOM_HEIGHT, VERTICAL_TRANSLATE_BACK_BOTTOM_Y, VERTICAL_TRANSLATE_FRONT_Y, false);
			IPsArtLayer back2 = AddJpegAreaToWebDoc(jpegDoc, webDoc, VERTICAL_BACK_TOP_X, VERTICAL_BACK_TOP_Y, VERTICAL_BACK_TOP_WIDTH, VERTICAL_BACK_TOP_HEIGHT, VERTICAL_TRANSLATE_BACK_TOP_Y, VERTICAL_TRANSLATE_FRONT_Y, false);

			layersToMerge.Clear();
			layersToMerge.Add(back1);
			layersToMerge.Add(back2);
			IPsArtLayer backMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			backMerged.Name = "Back";

			IPsLayerStyle layerStyle = app.CreateLayerStyle();
			IPsBevelEmbossStyle bevelStyle = layerStyle.AddBevelEmbossStyle();
			bevelStyle.Size = 20;
			backMerged.ApplyLayerStyle(layerStyle);

			IPsArtLayer backLeftEndLayer = webDoc.ArtLayers.AddNormalLayer("Back Left End");
			DrawFilledRectangle(webDoc, 1329, 135, 1962, 185, "Back Left End");

			IPsArtLayer backRightEndLayer = webDoc.ArtLayers.AddNormalLayer("Back Right End");
			DrawFilledRectangle(webDoc, 1329, 1750, 1962, 1800, "Back Right End");

			layersToMerge.Clear();
			layersToMerge.Add(backLeftEndLayer);
			layersToMerge.Add(backMerged);
			backMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			layersToMerge.Clear();
			layersToMerge.Add(backRightEndLayer);
			layersToMerge.Add(backMerged);
			backMerged = (IPsArtLayer)webDoc.MergeLayers(layersToMerge);
			backMerged.Name = "Back";

			backMerged.Rotate(15.0, EPsAnchorPosition.psBottomRight);

			backMerged.MoveAfter(frontMerged);
			backMerged.Translate(-440, 120);

			return webDoc;
		}

		private IPsArtLayer AddJpegAreaToWebDoc(IPsDocument jpegDoc, IPsDocument webDoc, int x, int y, int width, int height, int translateX, int translateY, bool applyBevel)
		{
			app.ActiveDocument = jpegDoc;
			jpegDoc.Selection.Select(new Rectangle(new Point(x, y), new Size(new Point(width, height))));
			jpegDoc.Selection.Copy(false);

			// Add to the template, resize, and place.
			app.ActiveDocument = webDoc;
			webDoc.Paste(false);

			if (applyBevel)
			{
				IPsLayerStyle layerStyle = app.CreateLayerStyle();
				IPsBevelEmbossStyle bevelStyle = layerStyle.AddBevelEmbossStyle();
				bevelStyle.Size = 20;
				webDoc.ActiveLayer.ArtLayer.ApplyLayerStyle(layerStyle);
			}

			//webDoc.ActiveLayer.Resize(634 / 800 * 100, 1520 / 1600 * 100, EPsAnchorPosition.psTopLeft);
			webDoc.ActiveLayer.Translate(translateX, translateY);

			return (IPsArtLayer)webDoc.ActiveLayer;
		}

		private string getWebTemplateFileName(string backgroundPath, string productCode)
		{
			string fileName = "DEF.psd";
			string productCategory = productCode.Substring(0, 3);

			if (File.Exists(backgroundPath + "\\" + productCode + ".psd"))
			{
				fileName = productCode + ".psd";
			}
			else if (File.Exists(backgroundPath + "\\" + productCode + ".txt"))
			{
				fileName = File.ReadAllText(backgroundPath + "\\" + productCode + ".txt") + ".psd";
			}
			else if (File.Exists(backgroundPath + "\\" + productCategory + ".psd"))
			{
				fileName = productCategory + ".psd";
			}

			return fileName;
		}

		private IPsPathItem DrawFilledRectangle(IPsDocument doc, int x1, int y1, int x2, int y2, string name)
		{
			List<IPsSubPathInfo> lineSubPaths = new List<IPsSubPathInfo>();
			IPsPathItem rectangle;

			app.ForegroundColor = app.CreateSolidColor(53, 24, 31);

			IPsPathPointInfo endPoint1 = app.CreatePathPointInfo();
			endPoint1.Kind = EPsPointKind.psCornerPoint;
			//endPoint1.Anchor = new PointF(0, 0);
			endPoint1.Anchor = new PointF((float)x1 / 300 * 72, (float)y1 / 300 * 72);
			endPoint1.LeftDirection = endPoint1.Anchor;
			endPoint1.RightDirection = endPoint1.Anchor;

			IPsPathPointInfo endPoint2 = app.CreatePathPointInfo();
			endPoint2.Kind = EPsPointKind.psCornerPoint;
			//endPoint2.Anchor = new PointF(100, 0);
			endPoint2.Anchor = new PointF((float)x2 / 300 * 72, (float)y1 / 300 * 72);
			endPoint2.LeftDirection = endPoint2.Anchor;
			endPoint2.RightDirection = endPoint2.Anchor;

			IPsPathPointInfo endPoint3 = app.CreatePathPointInfo();
			endPoint3.Kind = EPsPointKind.psCornerPoint;
			//endPoint3.Anchor = new PointF(100, 200);
			endPoint3.Anchor = new PointF((float)x2 / 300 * 72, (float)y2 / 300 * 72);
			endPoint3.LeftDirection = endPoint3.Anchor;
			endPoint3.RightDirection = endPoint3.Anchor;

			IPsPathPointInfo endPoint4 = app.CreatePathPointInfo();
			endPoint4.Kind = EPsPointKind.psCornerPoint;
			//endPoint4.Anchor = new PointF(0, 200);
			endPoint4.Anchor = new PointF((float)x1 / 300 * 72, (float)y2 / 300 * 72);
			endPoint4.LeftDirection = endPoint4.Anchor;
			endPoint4.RightDirection = endPoint4.Anchor;

			IPsSubPathInfo subPath = app.CreateSubPathInfo();
			subPath.Operation = EPsShapeOperation.psShapeXOR;
			subPath.Closed = true;

			subPath.AddPathPointInfo(endPoint1);
			subPath.AddPathPointInfo(endPoint2);
			subPath.AddPathPointInfo(endPoint3);
			subPath.AddPathPointInfo(endPoint4);
			lineSubPaths.Add(subPath);

			IPsSolidColor fillColor = app.CreateSolidColor(53, 24, 31);

			app.ActiveDocument = doc;
			rectangle = doc.PathItems.Add(name, lineSubPaths);
			//line.StrokePath(EPsToolType.psBrush, false);
			rectangle.FillPath(fillColor, EPsBlendMode.psOverlay, 100, false, 0, false, true);

			return rectangle;
		}

		private void DrawLine(IPsDocument doc, int x1, int y1, int x2, int y2, string name)
		{
			List<IPsSubPathInfo> lineSubPaths = new List<IPsSubPathInfo>();
			IPsPathItem line;

			IPsPathPointInfo endPoint1 = app.CreatePathPointInfo();
			endPoint1.Kind = EPsPointKind.psCornerPoint;
			endPoint1.Anchor = new PointF((float)x1 / 300 * 72, (float)y1 / 300 * 72);
			endPoint1.LeftDirection = endPoint1.Anchor;
			endPoint1.RightDirection = endPoint1.Anchor;

			IPsPathPointInfo endPoint2 = app.CreatePathPointInfo();
			endPoint2.Kind = EPsPointKind.psCornerPoint;
			endPoint2.Anchor = new PointF((float)x2 / 300 * 72, (float)y2 / 300 * 72);
			endPoint2.LeftDirection = endPoint2.Anchor;
			endPoint2.RightDirection = endPoint2.Anchor;

			IPsSubPathInfo subPath = app.CreateSubPathInfo();
			subPath.Operation = EPsShapeOperation.psShapeXOR;
			subPath.Closed = false;
			subPath.AddPathPointInfo(endPoint1);
			subPath.AddPathPointInfo(endPoint2);
			lineSubPaths.Add(subPath);

			line = doc.PathItems.Add(name, lineSubPaths);
			line.StrokePath(EPsToolType.psPencil, false);
		}

		private void CloseOpenDocuments()
		{
			IPsDocuments documents = app.Documents;
			foreach (var document in documents)
			{
				foreach (var docmement in document)
				{
					docmement.Close(EPsSaveOptions.psDoNotSaveChanges);
				}
			}
		}

		private void FilesToConvertListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			RemoveItemButton.Enabled = (FilesToConvertListBox.SelectedIndices.Count > 0);
			ConvertButton.Enabled = (FilesToConvertListBox.Items.Count > 0);
		}

		private void RemoveItemButton_Click(object sender, EventArgs e)
		{
			for (int i = FilesToConvertListBox.SelectedIndices.Count - 1; i >= 0; i--)
			{
				int index = FilesToConvertListBox.SelectedIndices[i];
				FilesToConvertListBox.Items.RemoveAt(index);
			}
		}

		private void RadioButtonCheckedChanged(object sender, EventArgs e)
		{

		}
	}
}
