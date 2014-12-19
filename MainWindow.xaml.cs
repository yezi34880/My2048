﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace My2048
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //标识每个方块，
        int[,] Block = new int[4, 4]
        {
            {0,0,0,0},
            {0,0,0,0},
            {0,0,0,0},
            {0,0,0,0}
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewNum();
            NewNum();
        }
        /// <summary>
        /// 随机生成新数字
        /// </summary>
        private void NewNum()
        {
            Random random = new Random();
            int num = random.Next(0, 9) > 2 ? 2 : 4;//随机生成2、4

            int nullnum = 0;//剩余空格数，随机确定生成位置
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (Block[i, j] == 0)
                        nullnum++;
            if (nullnum < 1)
            {
                return;
            }

            int index = random.Next(1, nullnum);
            nullnum = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (Block[i, j] == 0)
                    {
                        nullnum++;
                        if (nullnum != index) continue;
                        Block[i, j] = num;

                        Color backColor;
                        switch (num)
                        {
                            case 2:
                                backColor = Colors.LightPink;
                                break;
                            default:
                                backColor = Colors.LightSalmon;
                                break;
                        }

                        #region 动画效果
                        Label lbl = (Label)grdMain.Children
                            .Cast<UIElement>()
                            .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
                        lbl.Content = num.ToString();
                        lbl.Background = new SolidColorBrush(backColor);
                        DoubleAnimation opacity = new DoubleAnimation();
                        opacity.From = 0;
                        opacity.To = 1;
                        Duration duration = new Duration(TimeSpan.FromMilliseconds(500));
                        opacity.Duration = duration;
                        lbl.BeginAnimation(Label.OpacityProperty, opacity);
                        #endregion

                        if (isGameOver())
                        {
                            MessageBox.Show("Game Over");
                        }
                    }
        }

        /// <summary>
        /// 没有可移动的方块，游戏结束
        /// </summary>
        /// <returns></returns>
        private bool isGameOver()
        {
            //行没有相同或0
            for (int row = 0; row < 4; row++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Block[row, i] == 0)
                    {
                        return false;
                    }
                    else
                    {
                        for (int j = i + 1; j < 4; j++)
                        {
                            if (Block[row, i] == Block[row, j])
                            {
                                return false;
                            }
                            else
                            {
                                if (Block[row, j] > 0)
                                    break;
                            }
                        }
                    }
                }
            }

            //列没有相同或0
            for (int col = 0; col < 4; col++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Block[i, col] == 0)
                    {
                        return false;
                    }
                    else
                    {
                        for (int j = i + 1; j < 4; j++)
                        {
                            if (Block[i, col] == Block[j, col])
                            {
                                return false;
                            }
                            else
                            {
                                if (Block[j, col] > 0)
                                    break;
                            }
                        }
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// 分数改变 动画效果
        /// </summary>
        /// <param name="scores"></param>
        private void ScoresChanged(int scores)
        {
            if (scores > 0)
            {
                lblPlus.Content = String.Format("+{0}", scores);

                DoubleAnimation top = new DoubleAnimation();
                DoubleAnimation opacity = new DoubleAnimation();
                opacity.AutoReverse = true;
                opacity.From = 0;
                opacity.To = 1;
                top.From = 0;
                top.To = -30;
                Duration duration = new Duration(TimeSpan.FromMilliseconds(500));
                top.Duration = duration;
                opacity.Duration = duration;
                tt.BeginAnimation(TranslateTransform.YProperty, top);
                lblPlus.BeginAnimation(Label.OpacityProperty, opacity);

                lblScores.Content = int.Parse(lblScores.Content.ToString()) + scores;
            }
        }
        /// <summary>
        /// 枚举四个方向
        /// </summary>
        enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        /// <summary>
        /// 根据数组生成界面方块
        /// </summary>
        private void creatBlock(Direction deraction)
        {
            foreach (var uiele in grdMain.Children)
            {
                Label lbl = uiele as Label;
                if (lbl == null) continue;
                int row = Grid.GetRow(lbl);
                int col = Grid.GetColumn(lbl);

                int oldnum = (lbl.Content ?? "").ToString() == "" ? 0 : int.Parse((lbl.Content ?? "0").ToString());

                lbl.Content = Block[row, col] == 0 ? "" : Block[row, col].ToString();


                Color backColor;
                switch (Block[row, col])
                {
                    case 2:
                        backColor = Colors.LightPink;
                        break;
                    case 4:
                        backColor = Colors.LightSalmon;
                        break;
                    case 8:
                        backColor = Colors.Tomato;
                        break;
                    case 16:
                        backColor = Colors.Violet;
                        break;
                    case 32:
                        backColor = Colors.HotPink;
                        break;
                    case 64:
                        backColor = Colors.DarkOrchid;
                        break;
                    case 128:
                        backColor = Colors.Magenta;
                        break;
                    case 256:
                        backColor = Colors.MediumVioletRed;
                        break;
                    case 512:
                        backColor = Colors.PaleGreen;
                        break;
                    case 1024:
                        backColor = Colors.Lime;
                        break;
                    case 2048:
                        backColor = Colors.LightSkyBlue;
                        break;
                    case 4096:
                        backColor = Colors.RoyalBlue;
                        break;
                    default:
                        backColor = Colors.LawnGreen;
                        break;
                }
                lbl.Background = Block[row, col] == 0
                    ? new SolidColorBrush(Colors.White)
                    : new SolidColorBrush(backColor);

                if (oldnum != Block[row, col])
                {
                    TranslateTransform t = lbl.RenderTransform as TranslateTransform;

                    DoubleAnimation opacity = new DoubleAnimation();
                    opacity.From = 0;
                    opacity.To = 1;
                    Duration duration = new Duration(TimeSpan.FromMilliseconds(300));
                    opacity.Duration = duration;
                    lbl.BeginAnimation(Label.OpacityProperty, opacity);

                    DoubleAnimation offset = new DoubleAnimation();
                    offset.Duration = duration;

                    switch (deraction)
                    {
                        case Direction.Right:
                            offset.From = -30;
                            offset.To = 0;
                            t.BeginAnimation(TranslateTransform.XProperty, offset);
                            break;
                        case Direction.Left:
                            offset.From = 30;
                            offset.To = 0;
                            t.BeginAnimation(TranslateTransform.XProperty, offset);
                            break;
                        case Direction.Up:
                            offset.From = 30;
                            offset.To = 0;
                            t.BeginAnimation(TranslateTransform.YProperty, offset);
                            break;
                        default:
                            offset.From = -30;
                            offset.To = 0;
                            t.BeginAnimation(TranslateTransform.YProperty, offset);
                            break;
                    }

                }


            }
        }

        #region 方向按钮
        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            Left();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            Down();
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            Right();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            Up();
        }
        #endregion

        #region 移动
        private void Left()
        {
            bool isMove = false;
            int scores = 0;
            for (int row = 0; row < 4; row++)
                for (int i = 0; i < 4; i++)
                    for (int j = i + 1; j < 4; j++)
                    {
                        if (Block[row, i] == Block[row, j])
                        {
                            isMove = Block[row, i] > 0 ? true : isMove;

                            Block[row, i] *= 2;
                            Block[row, j] = 0;
                            scores += Block[row, i];
                            break;
                        }
                        if (Block[row, j] > 0)
                        {
                            break;
                        }
                    }

            ScoresChanged(scores);

            for (int row = 0; row < 4; row++)
                for (int i = 3; i > 0; i--)
                    if (Block[row, i - 1] == 0)
                    {
                        isMove = Block[row, i] > 0 ? true : isMove;

                        Block[row, i - 1] = Block[row, i];
                        Block[row, i] = 0;
                        if (i < 3)
                        {
                            for (int j = i + 1; j < 4; j++)
                            {
                                Block[row, j - 1] = Block[row, j];
                                Block[row, j] = 0;
                            }
                        }
                    }

            if (isMove == false)
            {
                return;
            }

            creatBlock(Direction.Left);
            NewNum();

        }

        private void Down()
        {
            bool isMove = false;
            int scores = 0;

            for (int col = 0; col < 4; col++)
                for (int i = 3; i > -1; i--)
                    for (int j = i - 1; j > -1; j--)
                    {
                        if (Block[i, col] == Block[j, col])
                        {
                            isMove = Block[i, col] > 0 ? true : isMove;

                            Block[i, col] *= 2;
                            Block[j, col] = 0;

                            scores += Block[i, col];
                            break;
                        }
                        if (Block[j, col] > 0)
                        {
                            break;
                        }
                    }
            for (int col = 0; col < 4; col++)
                for (int i = 0; i < 3; i++)
                    if (Block[i + 1, col] == 0)
                    {
                        isMove = Block[i, col] > 0 ? true : isMove;

                        Block[i + 1, col] = Block[i, col];
                        Block[i, col] = 0;

                        if (i > 0)
                        {
                            for (int j = i - 1; j > -1; j--)
                            {
                                Block[j + 1, col] = Block[j, col];
                                Block[j, col] = 0;
                            }
                        }
                    }

            ScoresChanged(scores);

            if (isMove == false)
            {
                return;
            }

            creatBlock(Direction.Down);
            NewNum();

        }

        private void Right()
        {
            bool isMove = false;
            int scores = 0;
            for (int row = 0; row < 4; row++)
                for (int i = 3; i > -1; i--)
                    for (int j = i - 1; j > -1; j--)
                    {
                        if (Block[row, i] == Block[row, j])
                        {
                            isMove = Block[row, i] > 0 ? true : isMove;

                            Block[row, i] *= 2;
                            Block[row, j] = 0;
                            scores += Block[row, i];
                            break;
                        }
                        if (Block[row, j] > 0)
                        {
                            break;
                        }
                    }
            for (int row = 0; row < 4; row++)
                for (int i = 0; i < 3; i++)
                    if (Block[row, i + 1] == 0)
                    {
                        isMove = Block[row, i] > 0 ? true : isMove;

                        Block[row, i + 1] = Block[row, i];
                        Block[row, i] = 0;

                        if (i > 0)
                        {
                            for (int j = i - 1; j > -1; j--)
                            {
                                Block[row, j + 1] = Block[row, j];
                                Block[row, j] = 0;
                            }
                        }
                    }

            ScoresChanged(scores);

            if (isMove == false)
            {
                return;
            }

            creatBlock(Direction.Right);
            NewNum();

        }

        private void Up()
        {
            bool isMove = false;
            int scores = 0;
            for (int col = 0; col < 4; col++)
                for (int i = 0; i < 4; i++)
                    for (int j = i + 1; j < 4; j++)
                    {
                        if (Block[i, col] == Block[j, col])
                        {
                            isMove = Block[i, col] > 0 ? true : isMove;

                            Block[i, col] *= 2;
                            Block[j, col] = 0;
                            scores += Block[i, col];

                            break;
                        }
                        if (Block[j, col] > 0)
                        {
                            break;
                        }
                    }
            for (int col = 0; col < 4; col++)
                for (int i = 3; i > 0; i--)
                    if (Block[i - 1, col] == 0)
                    {
                        isMove = Block[i, col] > 0 ? true : isMove;

                        Block[i - 1, col] = Block[i, col];
                        Block[i, col] = 0;
                        if (i < 3)
                        {
                            for (int j = i + 1; j < 4; j++)
                            {
                                Block[j - 1, col] = Block[j, col];
                                Block[j, col] = 0;
                            }
                        }
                    }

            ScoresChanged(scores);
            if (isMove == false)
            {
                return;
            }

            creatBlock(Direction.Up);
            NewNum();

        }

        #endregion

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch(e.Key)
            {
                case System.Windows.Input.Key.Up: Up(); break;
                case System.Windows.Input.Key.Down: Down(); break;
                case System.Windows.Input.Key.Right: Right(); break;
                case System.Windows.Input.Key.Left: Left(); break;
            }
        }
    }
}
