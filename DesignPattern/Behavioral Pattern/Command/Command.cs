using System;
using System.Collections.Generic;
using CommonClassLibary;

namespace DesignPattern.Command
{
    /// <summary>
    /// 定義:
    /// 將命令封裝成物件，讓使用者可以動態執行命令
    ///
    /// 角色:
    /// Invoker: 定義接收和執行命令的類別 [Invoker]
    /// Command: 定義執行命令的抽象類別 [CommandClass]
    /// ConcreteCommand: 繼承至Command的抽象類別，並實作執行命令方法 [GoAheadCommand] [TurnLeftCommand] [TurnRightCommand]
    /// Receiver: 定義命令的物件 [ReceiverRobot]
    ///
    /// 範例來源:
    /// https://xyz.cinc.biz/2013/07/command-pattern.html
    /// </summary>
    public class Command : IExecute
    {
        public void Main()
        {
            // 初始化各個物件
            Invoker invoker = new Invoker(); // 發命令物件

            ReceiverRobot robot = new ReceiverRobot(); // 執行命令物件

            GoAheadCommand cmd_go_ahead = new GoAheadCommand(robot); // 向前走指令
            TurnLeftCommand cmd_turn_left = new TurnLeftCommand(robot); // 左轉指令
            TurnRightCommand cmd_turn_right = new TurnRightCommand(robot); // 右轉指令

            // 設定要執行的命令
            invoker.SetCommand(cmd_go_ahead);
            invoker.SetCommand(cmd_go_ahead);
            invoker.SetCommand(cmd_turn_left);
            invoker.SetCommand(cmd_go_ahead);
            invoker.SetCommand(cmd_turn_right);

            // 開始執行命令
            invoker.Run();
        }
    }

    /// <summary>
    /// 用來發出命令的類別 (Invoker)
    /// </summary>
    internal class Invoker
    {
        private IList<CommandClass> cmds = new List<CommandClass>();

        public void SetCommand(CommandClass command)
        {
            cmds.Add(command);
        }

        public void Run()
        {
            foreach (CommandClass command in cmds)
            {
                command.Execute();
            }
        }
    }

    /// <summary>
    /// 命令的抽像類別，用來衍生各種命令，建構時，須設定實際執行命令的物件 (Command)
    /// </summary>
    internal abstract class CommandClass
    {
        protected ReceiverRobot robot;

        // 設定實際執行命令的物件
        public CommandClass(ReceiverRobot robot)
        {
            this.robot = robot;
        }

        // 用來呼叫執行命令的物件，開始執行命令
        abstract public void Execute();
    }

    /// <summary>
    /// 向前走一步的命令 (ConcreteCommand)
    /// </summary>
    internal class GoAheadCommand : CommandClass
    {
        public GoAheadCommand(ReceiverRobot robot)
            : base(robot)
        {
        }

        public override void Execute()
        {
            robot.GoAhead();
        }
    }

    /// <summary>
    /// 向左轉的命令 (ConcreteCommand)
    /// </summary>
    internal class TurnLeftCommand : CommandClass
    {
        public TurnLeftCommand(ReceiverRobot robot)
            : base(robot)
        {
        }

        public override void Execute()
        {
            robot.TurnLeft();
        }
    }

    /// <summary>
    /// 向右轉的命令 (ConcreteCommand)
    /// </summary>
    internal class TurnRightCommand : CommandClass
    {
        public TurnRightCommand(ReceiverRobot robot)
            : base(robot)
        {
        }

        public override void Execute()
        {
            robot.TurnRight();
        }
    }

    /// <summary>
    /// 實際執行命令的物件 (ConcreteCommand)
    /// </summary>
    internal class ReceiverRobot
    {
        public void GoAhead()
        {
            Console.WriteLine("向前走一步");
        }

        public void TurnLeft()
        {
            Console.WriteLine("向左轉");
        }

        public void TurnRight()
        {
            Console.WriteLine("向右轉");
        }
    }
}