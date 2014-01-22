using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public class DebugPanel : IGuiComponent {
        private Label fps;
        private Label debugText;
        private Panel parentPanel;

        private Queue<float> _lastFrametimes;

        public static DebugPanel ActiveInstance;

        public DebugPanel()
        {
            _lastFrametimes = new Queue<float>();
            ActiveInstance = this;
        }

        public void CreateGui(GUI gui) {

            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            parentPanel = new Panel("DP_parent") {
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                Size = new Size(200, 100),
                Location = new Point(gw - 220, 20),
                ResizeMode = ResizeModes.None
            };
            fps = new Label("DP_fps") {
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White
                },
                Location = new Point(5, 5),
                MaxSize = new Size(190, 45),
                AutoSize = true
            };
            debugText = new Label("DP_debugtext") {
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                },
                Location = new Point(5, 50),
                MaxSize = new Size(190, 45),
                AutoSize = true,
                Text = "Debug: "
            };

            parentPanel.SetBackgroundTexture(parentPanel.Skin.SubSkins["BlackPanelSkin40"]);

            parentPanel.Controls.Add(fps);
            parentPanel.Controls.Add(debugText);
            gui.Controls.Add(parentPanel);

            this.Visible = false;
        }

        /// <summary>
        /// Returns whether or not the panel is currently visible
        /// </summary>
        public bool Visible {
            get { return this.parentPanel.Visible; }
            set { this.parentPanel.Visible = value; }
        }

        /// <summary>
        /// Sets the "Debug:" field to the specified string, for debug purposes
        /// </summary>
        /// <param name="text">Text to set</param>
        public void SetDebugText(string text) {
            this.debugText.Text = "Debug:" + text;
        }
        /// <summary>
        /// Appends the specified string to the "Debug: " field
        /// </summary>
        /// <param name="text">Text to append</param>
        public void Write(string text) { this.debugText.Text += text; }
        /// <summary>
        /// Appends the specified string to the "Debug: " field after a newline
        /// </summary>
        /// <param name="text">Text to add</param>
        public void WriteLine(string text) { this.debugText.Text += "\n" + text; }

        /// <summary>
        /// Updates the FPS indicator
        /// </summary>
        /// <param name="frametime">The elapsed milliseconds since last update</param>
        public void UpdateFPS(float frametime) {
            if (frametime != 0) {
                _lastFrametimes.Enqueue(frametime);
            }
            if (_lastFrametimes.Count > 60) { _lastFrametimes.Dequeue(); }
            if (_lastFrametimes.Count == 0) { return; }
            float total = 0;
            foreach (float l in _lastFrametimes) { total += l; }
            float average = total / _lastFrametimes.Count;

            float millisecondsPerSecond = 1000.0f;
            float frameSeconds = (average) / millisecondsPerSecond;
            float framesPerSecond = 1.0f / frameSeconds;
            fps.Text = framesPerSecond.ToString().Substring(0, Math.Min(4, framesPerSecond.ToString().Length)) + " fps";
        }
    }
}
