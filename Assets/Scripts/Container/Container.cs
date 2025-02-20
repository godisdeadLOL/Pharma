using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ContainerSolution
{
    public IngredientData ingredientData;

    public int Amount;
    // public float Grind; 

    public float GetMass()
    {
        return ingredientData.density * Amount;
    }

    public ContainerSolution(IngredientData ingredientData, int amount)
    {
        this.ingredientData = ingredientData;
        this.Amount = amount;
    }
}


public class Container : MonoBehaviour
{
    public UnityAction<int> OnAmountChanged;
    public UnityAction<ContainerSolution> OnSolutionAdded;

    public List<ContainerSolution> solutions = new();

    public Color ColorOverride { get; set; }
    public float ColorOverrideLerp { get; set; }

    [field: SerializeField] public bool ApplyFilter { get; set; } = false;
    [field: SerializeField] public IngredientData Filter { get; set; } = null;


    public int volume;

    private int _transferOffset = 0;

    private void AdvanceOffset()
    {
        if (solutions.Count == 0) _transferOffset = 0;
        else if (_transferOffset > solutions.Count) _transferOffset = (_transferOffset - solutions.Count) % solutions.Count;
        else _transferOffset = (_transferOffset + 1) % solutions.Count;
    }

    public void AddSolution(ContainerSolution added)
    {
        OnAmountChanged?.Invoke(added.Amount);
        OnSolutionAdded?.Invoke(added);

        foreach (var solution in solutions)
        {
            if (solution.ingredientData == added.ingredientData)
            {
                solution.Amount += added.Amount;
                return;
            }
        }

        solutions.Add(added);
    }

    public void Transfer(Container target, int amount)
    {
        if (target.ApplyFilter && solutions.Count > 0)
        {
            if(target.Filter != solutions[0].ingredientData) return; // допустим в емкости только один ингредиент
        }

        OnAmountChanged?.Invoke(-amount);

        while (amount > 0 && solutions.Count > 0)
        {
            // переместить субстанции
            for (int i = 0; i < solutions.Count; i++)
            {
                int n = (_transferOffset + i) % solutions.Count;
                ContainerSolution solution = solutions[n];

                solution.Amount -= 1;
                target.AddSolution(new ContainerSolution(solution.ingredientData, 1));

                amount--;
                if (amount == 0) break;
            }

            AdvanceOffset();

            // удалить пустые ссылки
            for (int i = solutions.Count - 1; i >= 0; i--)
            {
                ContainerSolution solution = solutions[i];
                if (solution.Amount == 0) solutions.RemoveAt(i);
            }
        }
    }

    public float GetMass()
    {
        float total = 0;

        foreach (var solution in solutions)
        {
            total += solution.GetMass();
        }

        return total;
    }

    public int GetAmount()
    {
        int total = 0;

        foreach (var solution in solutions)
        {
            total += solution.Amount;
        }

        return total;
    }

    public Color GetColor()
    {
        float total = GetAmount();
        Color color = Color.black;

        foreach (var solution in solutions)
        {
            color += solution.ingredientData.color * solution.Amount / total;
        }

        return Color.Lerp(color, ColorOverride, ColorOverrideLerp);
    }

    public void ForceUpdateColor()
    {
        OnAmountChanged?.Invoke(0);
    }
}
