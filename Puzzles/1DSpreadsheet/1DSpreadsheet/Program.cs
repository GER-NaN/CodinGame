using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// https://www.codingame.com/ide/puzzle/1d-spreadsheet
/// </summary>
class Solution
{
    static void Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        List<Row> rows = new List<Row>();

        for (int i = 0; i < N; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            string operation = inputs[0];
            string arg1 = inputs[1];
            string arg2 = inputs[2];

            var x = new Row(operation, arg1, arg2, rows);
            rows.Add(x);
            Console.Error.WriteLine(x);
        }

        foreach (Row r in rows)
        {
            Console.WriteLine(r.GetValue());
        }
    }
}

class Row
{
    public Row(string o, string a1, string a2, List<Row> sheet)
    {
        Operation = o;
        Arg1 = a1;
        Arg2 = a2;
        Sheet = sheet;
    }

    public override string ToString()
    {
        return Operation + " " + Arg1 + " " + Arg2;
    }

    public string GetValue()
    {
        if (_calculated)
        {
            return _calculatedValue;
        }
        var value = "";
        int a1;
        int a2;

        switch (this.Operation)
        {
            case "VALUE":
                if (Arg1.Contains("$"))
                {
                    int refIndex = int.Parse(Arg1.Replace("$", ""));
                    Row refRow = Sheet[refIndex];
                    string refRowValue = refRow.GetValue();
                    a1 = int.Parse(refRowValue);

                }
                else
                {
                    a1 = int.Parse(Arg1);
                }

                value = a1.ToString();
                break;

            case "ADD":

                if (Arg1.Contains("$"))
                {
                    int refIndex = int.Parse(Arg1.Replace("$", ""));
                    Row refRow = Sheet[refIndex];
                    string refRowValue = refRow.GetValue();
                    a1 = int.Parse(refRowValue);

                }
                else
                {
                    a1 = int.Parse(Arg1);
                }

                if (Arg2.Contains("$"))
                {
                    int refIndex = int.Parse(Arg2.Replace("$", ""));
                    Row refRow = Sheet[refIndex];
                    string refRowValue = refRow.GetValue();
                    a2 = int.Parse(refRowValue);

                }
                else
                {
                    a2 = int.Parse(Arg2);
                }

                value = (a1 + a2).ToString();
                break;

            case "SUB":
                if (Arg1.Contains("$"))
                {
                    int refIndex = int.Parse(Arg1.Replace("$", ""));
                    Row refRow = Sheet[refIndex];
                    string refRowValue = refRow.GetValue();
                    a1 = int.Parse(refRowValue);

                }
                else
                {
                    a1 = int.Parse(Arg1);
                }

                if (Arg2.Contains("$"))
                {
                    int refIndex = int.Parse(Arg2.Replace("$", ""));
                    Row refRow = Sheet[refIndex];
                    string refRowValue = refRow.GetValue();
                    a2 = int.Parse(refRowValue);

                }
                else
                {
                    a2 = int.Parse(Arg2);
                }

                value = (a1 - a2).ToString();
                break;
            case "MULT":
                if (Arg1.Contains("$"))
                {
                    int refIndex = int.Parse(Arg1.Replace("$", ""));
                    Row refRow = Sheet[refIndex];
                    string refRowValue = refRow.GetValue();
                    a1 = int.Parse(refRowValue);

                }
                else
                {
                    a1 = int.Parse(Arg1);
                }

                if (Arg2.Contains("$"))
                {
                    int refIndex = int.Parse(Arg2.Replace("$", ""));
                    Row refRow = Sheet[refIndex];
                    string refRowValue = refRow.GetValue();
                    a2 = int.Parse(refRowValue);

                }
                else
                {
                    a2 = int.Parse(Arg2);
                }

                value = (a1 * a2).ToString();
                break;
        }
        _calculatedValue = value;
        _calculated = true;
        return value;
    }

    private string _calculatedValue;
    private bool _calculated = false;

    public string Operation;
    public string Arg1;
    public string Arg2;
    public List<Row> Sheet;
}