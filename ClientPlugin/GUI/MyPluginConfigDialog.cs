using System;
using System.Text;
using Sandbox;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.GUI
{

    public class MyPluginConfigDialog : MyGuiScreenBase
    {
        private const string Caption = "AddMissingSearchBoxes Configuration";
        public override string GetFriendlyName() => "MyPluginConfigDialog_AddMissingSearchBoxes";

        private MyLayoutTable layoutTable;

        private MyGuiControlLabel factionsSearchEnabledLabel;
        private MyGuiControlCheckbox factionsSearchEnabledCheckbox;

        private MyGuiControlLabel chatSearchEnabledLabel;
        private MyGuiControlCheckbox chatSearchEnabledCheckbox;

        private MyGuiControlMultilineText infoText;
        private MyGuiControlButton closeButton;

        public MyPluginConfigDialog() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.5f, 0.7f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            CreateControls();
            LayoutControls();
        }

        private void CreateControls()
        {
            AddCaption(Caption);

            var config = Plugin.Instance.Config;
            CreateCheckbox(out factionsSearchEnabledLabel, out factionsSearchEnabledCheckbox, config.FactionsSearchboxEnabled, value => config.FactionsSearchboxEnabled = value, "Show Factions Searchbox", "");
            CreateCheckbox(out chatSearchEnabledLabel, out chatSearchEnabledCheckbox, config.ChatSearchboxEnabled, value => config.ChatSearchboxEnabled = value, "Show Chat Searchbox", "");
            // TODO: Create your UI controls here

            infoText = new MyGuiControlMultilineText
            {
                Name = "InfoText",
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                Text = new StringBuilder("\r\nConfig settings to hide unwanted searchboxes.")
            };

            closeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OnOk);
        }

        private void OnOk(MyGuiControlButton _) => CloseScreen();

        private void CreateCheckbox(out MyGuiControlLabel labelControl, out MyGuiControlCheckbox checkboxControl, bool value, Action<bool> store, string label, string tooltip)
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
            };

            checkboxControl = new MyGuiControlCheckbox(toolTip: tooltip)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = true,
                IsChecked = value
            };
            checkboxControl.IsCheckedChanged += cb => store(cb.IsChecked);
        }

        private void LayoutControls()
        {
            var size = Size ?? Vector2.One;
            layoutTable = new MyLayoutTable(this, -0.3f * size, 0.6f * size);
            layoutTable.SetColumnWidths(400f, 100f);
            // TODO: Add more row heights here as needed
            layoutTable.SetRowHeights(90f, 90f, 150f, 60f);

            var row = 0;

            layoutTable.Add(factionsSearchEnabledLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(factionsSearchEnabledCheckbox, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;

            layoutTable.Add(chatSearchEnabledLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(chatSearchEnabledCheckbox, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;

            // TODO: Layout your UI controls here

            layoutTable.Add(infoText, MyAlignH.Left, MyAlignV.Top, row, 0, colSpan: 2);
            row++;

            layoutTable.Add(closeButton, MyAlignH.Center, MyAlignV.Center, row, 0, colSpan: 2);
            // row++;
        }
    }
}