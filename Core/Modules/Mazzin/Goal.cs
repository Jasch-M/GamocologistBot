using System;

namespace Template.Modules.Mazzin;

/// <summary>
/// Represents a donation goal.
/// </summary>
public sealed class Goal
{
    /// <summary>
    /// Creates a new goal from a name and amount.
    /// Optionally, the goal can be set to be active or not.
    /// Optionally, the goal can be set to have been completed or not.
    /// </summary>
    /// <param name="name">
    /// The name of the goal.
    /// </param>
    /// <param name="value">
    /// The value of the goal.
    /// </param>
    /// <param name="isActive">
    /// Whether the goal is active or not.
    /// This is an optional parameter. The default value is false.
    /// If not specified, the goal is assumed to not be active.
    /// </param>
    /// <param name="isCompleted">
    /// Whether the goal is completed or not.
    /// This is an optional parameter. The default value is false.
    /// If not specified, the goal is assumed to not be completed.
    /// </param>
    public Goal(string name, int value, bool isActive = false, bool isCompleted = false)
    {
        Name = name;
        Value = value;
        IsActive = isActive;
        IsCompleted = isCompleted;
    }

    /// <summary>
    /// Creates a new goal from a goal string.
    /// The goal string must be in one of the following formats:
    /// GoalName,GoalValue,IsActive,IsCompleted or
    /// GoalName,GoalValue,IsActive or
    /// GoalName,GoalValue or
    /// GoalName
    /// </summary>
    /// <param name="goalStr">
    /// The goal string.
    /// </param>
    public Goal(string goalStr)
    {
        Goal goal = Parse(goalStr);
        Name = goal.Name;
        Value = goal.Value;
        IsActive = goal.IsActive;
        IsCompleted = goal.IsCompleted;
    }

    /// <summary>
    /// The name of the goal.
    /// The getter is public.
    /// The setter is internal.
    /// </summary>
    public string Name { get; internal set; }
    
    /// <summary>
    /// The value in dollars of the goal.
    /// The getter is public.
    /// The setter is internal.
    /// </summary>
    public int Value { get; internal set; }
    
    /// <summary>
    /// Whether the goal is active or not.
    /// The getter is public.
    /// The setter is internal.
    /// </summary>
    public bool IsActive { get; internal set; }
    
    /// <summary>
    /// Whether the goal is completed or not.
    /// The getter is public.
    /// The setter is internal.
    /// </summary>
    public bool IsCompleted { get; internal set; }

    /// <summary>
    /// Marks the goal as completed.
    /// </summary>
    public void Complete()
    {
        IsCompleted = true;
    }
    
    /// <summary>
    /// Marks the goal as active.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }
    
    /// <summary>
    /// Checks whether the goal has been achieved
    /// with the given donation amount.
    /// </summary>
    /// <param name="amount">
    /// The total donation amount.
    /// </param>
    /// <returns>
    /// True if the goal has been achieved.
    /// False if the goal has not been achieved.
    /// </returns>
    public bool IsReached(double amount)
    {
        return amount >= Value;
    }

    /// <summary>
    /// Returns a string representation of the goal.
    /// </summary>
    /// <returns>
    /// A string representation of the goal.
    /// This is the goal's <see cref="Name"/>.
    /// </returns>
    public override string ToString()
    {
        return $"{Name}";
    }

    /// <summary>
    /// Returns a string representation of the goal with all the values.
    /// </summary>
    /// <returns>
    /// A string representation of the goal which includes all the values.
    /// This is the goal's name, value, whether it is active, and whether it is completed in the format:
    /// <see cref="Name"/>, <see cref="Value"/>, <see cref="IsActive"/>, <see cref="IsCompleted"/>
    /// </returns>
    internal string ToStringFull()
    {
        string valueStr = Value.ToString();
        string activeStr = IsActive ? "Active" : "Inactive";
        string completedStr = IsCompleted ? "Completed" : "Incomplete";
        return $"{Name}, {valueStr}, {activeStr}, {completedStr}";
    }
    
    /// <summary>
    /// Checks whether a goal is equal to an object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare to.
    /// The object can be null.
    /// </param>
    /// <returns>
    /// If the object is null, false is returned.
    /// If the object is a goal, they are equal if they have the same name.
    /// If the object is a string, it is parsed and compared to the goal.
    /// If the parsing fails, false is returned.
    /// If the object is neither a goal nor a string, false is returned.
    /// </returns>
    public override bool Equals(object obj)
    {
        if (obj is not string objStr)
            return obj is Goal goal &&
                   Name == goal.Name;
        
        try
        {
            Goal goalInObj = new(objStr);
            return goalInObj.Name == Name;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    protected bool Equals(Goal other)
    {
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        string nameValue = Name;
        return nameValue.GetHashCode();
    }
    
    public static bool operator ==(Goal goal1, Goal goal2)
    {
        if (goal1 is null && goal2 is null)
            return true;

        if (goal1 is null || goal2 is null)
            return false;

        return goal1.Value == goal2.Value;
    }
    
    public static bool operator !=(Goal goal1, Goal goal2)
    {
        return !(goal1 == goal2);
    }
    
    public static implicit operator bool(Goal goal)
    {
        return goal.IsActive;
    }
    
    public static implicit operator Goal(string name)
    {
        return new Goal(name, 0);
    }
    
    public static implicit operator Goal(int value)
    {
        return new Goal("", value);
    }

    public static bool operator <(Goal goal1, Goal goal2)
    {
        return goal1.Value < goal2.Value;
    }
    
    public static bool operator >(Goal goal1, Goal goal2)
    {
        return goal1.Value > goal2.Value;
    }
    
    public static bool operator <=(Goal goal1, Goal goal2)
    {
        return goal1.Value <= goal2.Value;
    }
    
    public static bool operator >=(Goal goal1, Goal goal2)
    {
        return goal1.Value >= goal2.Value;
    }

    internal static Goal Parse(string input)
    {
        string[] parts = input.Split(',');
        switch (parts.Length)
        {
            case 1:
            {
                string goalName = parts[0];
                return new Goal(goalName, 0);
            }
            case 2:
            {
                string goalName = parts[0];
                string goalValueString = parts[1];
                bool wasParsedSuccessfullyValue = int.TryParse(goalValueString, out int goalValue);
                if (!wasParsedSuccessfullyValue)
                    throw new ArgumentException("The goal value must be an integer.");
                return new Goal(goalName, goalValue);
            }
            case 3:
            {
                string goalName = parts[0];
                string goalValueString = parts[1];
                string isActiveString = parts[2];
                bool wasParsedSuccessfullyValue = int.TryParse(goalValueString, out int goalValue);
                bool wasParsedSuccessfullyIsActive = bool.TryParse(isActiveString, out bool isActive);
                
                if (!wasParsedSuccessfullyValue || !wasParsedSuccessfullyIsActive)
                    throw new ArgumentException("The goal could not be parsed.");

                return new Goal(goalName, goalValue, isActive);
            }
            case 4:
            {
                string goalName = parts[0];
                string goalValueString = parts[1];
                string isActiveString = parts[2];
                string isCompletedString = parts[3];
                bool wasParsedSuccessfullyValue = int.TryParse(goalValueString, out int goalValue);
                bool wasParsedSuccessfullyIsActive = bool.TryParse(isActiveString, out bool isActive);
                bool wasParsedSuccessfullyIsCompleted = bool.TryParse(isCompletedString, out bool isCompleted);
                
                if (!wasParsedSuccessfullyValue || !wasParsedSuccessfullyIsActive || !wasParsedSuccessfullyIsCompleted)
                    throw new ArgumentException("The goal could not be parsed.");

                return new Goal(goalName, goalValue, isActive, isCompleted);
            }
            default:
                throw new ArgumentException("Invalid goal format");
        }
    }
}