using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ContainerSolution
{
    public IngredientData ingredientData;

    public int amount;

    public float GetMass()
    {
        return ingredientData.density * amount;
    }

    public ContainerSolution(IngredientData ingredientData, int amount)
    {
        this.ingredientData = ingredientData;
        this.amount = amount;
    }
}


public class Container : MonoBehaviour
{
    public UnityAction<int> OnAmountChanged;

    public List<ContainerSolution> solutions = new();

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
        OnAmountChanged?.Invoke(added.amount);

        foreach (var solution in solutions)
        {
            if (solution.ingredientData == added.ingredientData)
            {
                solution.amount += added.amount;
                return;
            }
        }

        solutions.Add(added);
    }

    public void Transfer(Container target, int amount)
    {
        OnAmountChanged?.Invoke(-amount);

        // todo: оптимизировать

        while (amount > 0 && solutions.Count > 0)
        {
            // переместить субстанции
            for (int i = 0; i < solutions.Count; i++)
            {
                int n = (_transferOffset + i) % solutions.Count;
                ContainerSolution solution = solutions[n];

                solution.amount -= 1;
                target.AddSolution(new ContainerSolution(solution.ingredientData, 1));

                amount--;
                if (amount == 0) break;
            }

            AdvanceOffset();

            // удалить пустые ссылки
            for (int i = solutions.Count - 1; i >= 0; i--)
            {
                ContainerSolution solution = solutions[i];
                if (solution.amount == 0) solutions.RemoveAt(i);
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
            total += solution.amount;
        }

        return total;
    }
}
