﻿using System;
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
    public partial class GameConsole : Window, IGuiComponent {

        private Dictionary<string, ConsoleCommand> commands;

        public static GameConsole ActiveInstance { get; private set; }

        public GameConsole() {
            commands = new Dictionary<string, ConsoleCommand>();
            ActiveInstance = this;
        }

        public override void Initialize() {
            //Add default commands
            builtins();

            this.Hide();
        }

        /// <summary>
        /// Appearify the console
        /// </summary>
        public override void Show() {
            base.Show();
            this.ParentPanel.GUI.MiyagiSystem.GUIManager.FocusedControl = this.entryBox;
        }

        public void WriteError(string error) {
            this.AddLabel(error, Colours.Red);
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
            this.AddLabel(text, Colours.Black);
        }
        private void AddLabel(string text, Colour col) {
            var label = new Label {
                Location = new Point(0, labelY),
                Text = "> " + text.Trim(),
                AutoSize = true,
                TextStyle = new TextStyle() {
                    Font = ResourceManager.Fonts["Courier"],
                    Multiline = true,
                    ForegroundColour = col
                },
                MaxSize = new Size(this.outputPanel.Width, this.outputPanel.Height)
            };
            label.SuccessfulHitTest += (s, e) => e.Cancel = true;
            this.outputPanel.Controls.Add(label);
            this.labelY += label.Height;
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
            string[] command = input.Split(' ');//new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (command.Length <= 0) { return; }
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
            commands.Add("echo", new ConsoleCommand((string[] args) => { 
                Echo(String.Join(" ", args)); 
            }, "Echoes the specified text."));

            commands.Add("version", new ConsoleCommand((string[] args) => { 
                Echo("Version: v" + Program.MAJOR_VERSION + "." + Program.MINOR_VERSION); 
            }, "Outputs the current game version."));

            commands.Add("list", new ConsoleCommand((string[] args) => {
                foreach (KeyValuePair<string, ConsoleCommand> kvp in commands) {
                    Echo(kvp.Key + ": " + kvp.Value.Description);
                }
            }, "Shows a list of all registered commands."));

            commands.Add("info", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { Echo("No command specified!"); return; }
                if (commands.ContainsKey(args[0])) {
                    Echo(commands[args[0]].Description);
                }
                else { Echo("No such command!"); }
            }, "Prints the description of the specified command."));
            commands.Add("help", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { Echo("Use \"list\" to see a list of all available commands."); return; }
                else {
                    foreach (string arg in args) {
                        if (commands.ContainsKey(arg)) {
                            Echo(arg+": " + commands[arg].Description);
                        }
                    }
                }
            }, "Shows help information."));
            commands.Add("crash", new ConsoleCommand((string[] args) => {
                throw new Exception("User has crashed the game.");
            }, "Crashes the game."));
            commands.Add("clear", new ConsoleCommand((string[] args) => { ClearScreen(); }, "Clears the console of previously printed text."));
            commands.Add("cls", new ConsoleCommand((string[] args) => { ClearScreen(); }, "Clears the console of previously printed text."));
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

        /// <summary>
        /// Clears the panel of all previously printed text.
        /// </summary>
        public void ClearScreen() {
            for (int i = 0; i < this.outputPanel.Controls.Count;) {
                if (this.outputPanel.Controls[i] is Label) {
                    Control c = this.outputPanel.Controls[i];
                    this.outputPanel.Controls.RemoveAt(i);
                }
                else { ++i; }
            }
            labelY = 0;
        }

        public override void Update(float frametime) {

        }
    }

    public class ConsoleCommand {

        public delegate void Command(params string[] args);

        public string Description;
        public string Usage;
        public Command Action;

        public ConsoleCommand(Command action) : this(action, "") { }
        public ConsoleCommand(Command action, string desc) : this(action, desc, "") { }
        public ConsoleCommand(Command action, string desc, string usage) {
            this.Action = action;
            this.Description = desc;
            this.Usage = usage;
        }

        public void Invoke(string[] args) {
            this.Action.Invoke(args);
        }
    }
}
