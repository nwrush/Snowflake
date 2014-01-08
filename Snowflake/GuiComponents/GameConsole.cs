using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;

using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public class GameConsole : GuiComponent {

        private int labelY;
        private Panel outputPanel;
        private Panel parentPanel;
        private TextBox entryBox;

        private Dictionary<string, ConsoleCommand> commands;
        public delegate void ConsoleCommand(params string[] args);

        public GameConsole() {
            commands = new Dictionary<string, ConsoleCommand>();

        }

        public override void CreateGui(MiyagiSystem system) {
            base.CreateGui(system);

            parentPanel = new Panel("GC_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = true,
                Size = new Size(516, 416),
                Movable = true,
                Location = new Point(200, 100),
                MinSize = new Size(4, 4),
                ResizeThreshold = new Thickness(4),
                BorderStyle = {
                    Thickness = new Thickness(4, 24, 4, 4)
                },
                Skin = ResourceManager.Skins["WindowSkin"]
            };
            this.outputPanel = new Panel("GC_OutputPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = true,
                Size = new Size(516, 360),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Location = new Point(0, 0),
                MinSize = new Size(0, 0),
                BorderStyle = {
                    Thickness = new Thickness(1, 1, 1, 1)
                },
                HScrollBarStyle = {
                    Extent = 16,
                    ThumbStyle = {
                        BorderStyle = {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                VScrollBarStyle = {
                    Extent = 16,
                    ThumbStyle = {
                        BorderStyle = {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                Skin = ResourceManager.Skins["PanelSkin"]
            };

            entryBox = new TextBox("GC_Entrybox") {
                Size = new Size(508, 32),
                Location = new Point(0, 360),
                Padding = new Thickness(9, 0, 8, 0),
                TextStyle = {
                    ForegroundColour = Colours.Black
                },
                TextBoxStyle = {
                    CaretStyle = {
                        Size = new Size(2, 16),
                        Colour = Colours.Black,
                    }
                },
                BorderStyle = {
                    Thickness = new Thickness(2, 2, 2, 2),
                },
                Skin = ResourceManager.Skins["PanelSkin"],
                ClearTextOnSubmit = true,
                DefocusOnSubmit = false
            };

            entryBox.Submit += (object sender, ValueEventArgs<string> e) => { this.Command(((TextBox)sender).Text); };
            entryBox.GotFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = true; };
            entryBox.LostFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = false; };
            entryBox.AutoCompleteSource = this.commands.Keys;

            parentPanel.ClientSizeChanged += (object sender, EventArgs e) => {
                entryBox.Width = parentPanel.Width - parentPanel.BorderStyle.Thickness.Left - parentPanel.BorderStyle.Thickness.Right;
                entryBox.Bottom = parentPanel.Height - parentPanel.BorderStyle.Thickness.Bottom - parentPanel.BorderStyle.Thickness.Top;
                outputPanel.Width = parentPanel.Width - parentPanel.BorderStyle.Thickness.Left - parentPanel.BorderStyle.Thickness.Right;
                outputPanel.Height = parentPanel.Height - entryBox.Height - parentPanel.BorderStyle.Thickness.Bottom - parentPanel.BorderStyle.Thickness.Top;

                foreach (Control c in outputPanel.Controls) {
                    c.MaxSize = new Size(outputPanel.Width, outputPanel.Height);
                }
            };
            parentPanel.Controls.Add(entryBox);
            parentPanel.Controls.Add(outputPanel);

            Console.WriteLine("Creating Console");
            gui.Controls.Add(parentPanel);

            // add the GUI to the GUIManager
            system.GUIManager.GUIs.Add(gui);
            
            //Add default commands
            builtins();

            this.Hide();
        }

        /// <summary>
        /// Appearify the console
        /// </summary>
        public void Show() {
            this.parentPanel.Visible = true;
            this.parentPanel.GUI.MiyagiSystem.GUIManager.FocusedControl = this.entryBox;
        }
        /// <summary>
        /// Disappearify the console
        /// </summary>
        public void Hide() {
            this.parentPanel.Visible = false;
        }
        /// <summary>
        /// Returns whether or not the console is currently visible
        /// </summary>
        public bool Visible { 
            get { return this.parentPanel.Visible; } 
            set { 
                this.parentPanel.Visible = value;
                if (value == true) { this.parentPanel.GUI.MiyagiSystem.GUIManager.FocusedControl = this.entryBox; }
            } 
        }

        /// <summary>
        /// Writes a line of text to the console (Alias for Echo)
        /// </summary>
        /// <param name="text">Text to write</param>
        public void WriteLine(string text) { Echo(text); }
        /// <summary>
        /// Writes a line of text to the console
        /// </summary>
        /// <param name="text"></param>
        public void Echo(string text) {
            this.AddLabel(text);
        }
        private void AddLabel(string text) {
            var label = new Label {
                Location = new Point(0, labelY),
                Text = "> " + text,
                AutoSize = true
            };
            label.SuccessfulHitTest += (s, e) => e.Cancel = true;
            this.outputPanel.Controls.Add(label);
            this.labelY += label.Size.Height;
            this.outputPanel.ScrollToBottom();
        }

        /// <summary>
        /// Appends text to the last written line
        /// </summary>
        /// <param name="text">Text to append</param>
        public void Write(string text) { AmendLast(text); }
        private void AmendLast(string text) {
            ((Label)this.outputPanel.Controls[this.outputPanel.Controls.Count - 1]).Text += text;
        }

        /// <summary>
        /// Registers a command that the console can execute
        /// </summary>
        /// <param name="commandName">The name of the command</param>
        /// <param name="command">The ConsoleCommand delegate to run when the command is typed</param>
        public void AddCommand(string commandName, ConsoleCommand command) {
            commands.Add(commandName, command);
        }

        /// <summary>
        /// Execute a command, echoing the command and its args to the console first
        /// </summary>
        /// <param name="command">Command to execute</param>
        public void Command(string command) { Echo(command); ExecuteCommand(command); }
        /// <summary>
        /// Execute a command without echoing it or its args (unless the command to run does so)
        /// </summary>
        /// <param name="input">Command to execute formatted as [CommandName] [Arg1] [Arg2] etc.</param>
        public void ExecuteCommand(string input) {
            string[] command = input.Split(' ');
            string commandName = command[0];
            string[] args = new string[command.Length - 1];
            Array.Copy(command, 1, args, 0, command.Length - 1); 

            if (commands.ContainsKey(commandName)) {
                commands[commandName].Invoke(args);
            }
            else {
                this.WriteLine("The command \""+commandName+"\" does not exist!");
            }
        }

        /// <summary>
        /// Setup builtin (no dependency on instances of other classes) ccommands.
        /// Commands that are predicated on the existence of other classes are defined elsewhere.
        /// </summary>
        private void builtins() {
            commands.Add("echo", (string[] args) => { Echo(String.Join(" ", args)); });
            commands.Add("version", (string[] args) => { Echo("Version: v" + Program.MAJOR_VERSION + "." + Program.MINOR_VERSION); });
        }

        /// <summary>
        /// Removes all registered commands.
        /// Use if many commands might have dependencies that are no longer available.
        /// </summary>
        public void Reset() {
            this.commands.Clear();
            builtins();
        }
        /// <summary>
        /// Remove a specific registered command.
        /// Use if a command might have dependencies that are no longer available.
        /// </summary>
        /// <param name="commandName">Command to remove</param>
        public void RemoveCommand(string commandName) {
            this.commands.Remove(commandName);
        }
    }
}
