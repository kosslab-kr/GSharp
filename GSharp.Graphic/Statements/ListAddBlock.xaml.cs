﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Statements;
using GSharp.Base.Objects.Numbers;
using GSharp.Graphic.Objects;
using System.Xml;
using GSharp.Graphic.Controls;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// ListAddBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ListAddBlock : PrevStatementBlock
    {
        public ListAddBlock()
        {
            InitializeComponent();
            InitializeBlock();
        }

        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }

        public override StatementBlock GetLastBlock()
        {
            if (NextConnectHole.StatementBlock == null)
            {
                return this;
            }

            return NextConnectHole.StatementBlock.GetLastBlock();
        }

        public GListAdd GListAdd
        {
            get
            {
                GSettableObject list = SettableObjectHole?.SettableObjectBlock?.GSettableObject;
                GObject obj = ObjectHole?.ObjectBlock?.GObject;

                if (list == null || obj == null)
                {
                    throw new ToObjectException("배열에 추가 블럭이 완성되지 않았습니다.", this);
                }

                return new GListAdd(list, obj);
            }
        }

        public override GStatement GStatement
        {
            get
            {
                return GListAdd;
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { SettableObjectHole, ObjectHole, NextConnectHole };
            }
        }

        public override List<GBase> ToGObjectList()
        {
            List<GBase> baseList = new List<GBase>();
            baseList.Add(GListAdd);

            List<GBase> nextList = NextConnectHole?.StatementBlock?.ToGObjectList();
            if (nextList != null)
            {
                baseList.AddRange(nextList);
            }

            return baseList;
        }

        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            var block = new ListAddBlock();

            XmlNodeList nodeList = element.SelectNodes("Holes/Hole");

            block.SettableObjectHole.SettableObjectBlock = LoadBlock(nodeList[0].SelectSingleNode("Block") as XmlElement, blockEditor) as SettableObjectBlock;
            block.ObjectHole.ObjectBlock = LoadBlock(nodeList[1].SelectSingleNode("Block") as XmlElement, blockEditor) as ObjectBlock;

            return block;
        }
    }
}
