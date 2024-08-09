namespace WpfCalculator.Model;
public class Calculator
{
    readonly HashSet<string> operators = new() { "+", "-", "*", "/", "=" };
    readonly HashSet<string> numbers = new() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };
    readonly HashSet<string> deleteCommands = new() { "←", "C", "CE" };

    public event Action<string>? OnResultUpdate;
    public event Action<string>? OnCalculationError;


    bool finishedState = false;

    List<string> parts = new();

    private string result = "0";

    public string Result
    {
        get { return result; }
        set
        {
            result = value;
            OnResultUpdate?.Invoke(value);
        }
    }

    public void AddNewChar(string nextChar)
    {
        if (finishedState && !operators.Contains(nextChar))
            ClearAll();

        finishedState = false;

        if (deleteCommands.Contains(nextChar))
        {
            if (nextChar == "←")
                DeleteCharFromEnd();

            if (nextChar == "C")
                ClearAll();
        }

        if (numbers.Contains(nextChar))
        {
            AddNumberToEnd(nextChar);
        }

        if (operators.Contains(nextChar))
        {
            if (nextChar == "=")
                Calculate();

            else
                AddOperatorToEnd(nextChar);
        }

        UpdateResult();
    }

    void AddNumberToEnd(string charToAdd)
    {
        if (parts.Count == 0)
        {
            if (charToAdd == ".")
                charToAdd = "0.";

            parts.Add(charToAdd);
            return;
        }

        string last = parts.Last();

        if (operators.Contains(last))
        {
            if (charToAdd == ".")
                charToAdd = "0.";

            parts.Add(charToAdd);
            return;
        }

        if (charToAdd == "." && last.Contains('.'))
            return;

        if (charToAdd != "." && last == "0")
            last = "";

        parts[^1] = last + charToAdd;
    }

    void AddOperatorToEnd(string charToAdd)
    {
        if (parts.Count == 0)
            return;

        string last = parts.Last();

        // last is also operator -> replace with the new one
        if (operators.Contains(last))
        {
            parts[^1] = charToAdd;
            return;
        }

        parts.Add(charToAdd);
    }

    void DeleteCharFromEnd()
    {
        if (parts.Count == 0)
            return;

        string last = parts.Last();
        parts[^1] = last.Remove(last.Length - 1);

        if (parts[^1] == "")
            parts.RemoveAt(parts.Count - 1);

    }

    void ClearAll()
    {
        parts.Clear();
        UpdateResult();
    }

    void UpdateResult()
    {
        Result = string.Join(" ", parts);

        if (Result == "")
            Result = "0";
    }

    void Calculate()
    {
        List<string> partsClone = new List<string>(parts);
        Dictionary<string, Func<double, double, double>> operatorsByOrder = new() {
                {"*", (a, b) => a * b},
                {"/", (a, b) => a / b},
                {"+", (a, b) => a + b},
                {"-", (a, b) => a - b}
            };

        foreach (var (op, func) in operatorsByOrder)
        {
            for (int i = 1; i < partsClone.Count - 1; i++)
            {
                if (partsClone[i] == op)
                {
                    var op1 = double.Parse(partsClone[i - 1]);
                    var op2 = double.Parse(partsClone[i + 1]);
                    double expressionResult = func(op1, op2);
                    if (double.IsInfinity(expressionResult) || double.IsNaN(expressionResult))
                    {
                        OnCalculationError?.Invoke("Divided by zero!");
                        return;
                    }
                    partsClone[i] = expressionResult.ToString();
                    partsClone.RemoveAt(i + 1);
                    partsClone.RemoveAt(i - 1);
                    i--;
                }
            }
        }

        parts = new List<string>(partsClone);
        finishedState = true;
    }


}
