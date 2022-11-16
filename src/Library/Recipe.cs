//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }

        public bool Cooked { get; private set; }

        private CountdownTimer Timer;
        private TimerAdapter timerClient;

        public Recipe()
        {
            this.Cooked = false;
            // this.Timer.Register(2000, this);
        }


        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }


        public int GetCookTime()
        {
            int time = 0;

            foreach (BaseStep step in this.steps)
            {
                time = time + step.Time;
            }

            return time;
        }

        public void Cook()
        {
            if (this.Cooked) throw new InvalidOperationException("Receta ya lista");

            this.StartCountTimer();

        }

        public void StartCountTimer()
        {
            this.timerClient = new TimerAdapter(this);

            this.Timer = new CountdownTimer();
            this.Timer.Register(this.GetCookTime(), timerClient);


        }

        public void TimeOut()
        {
            this.Cooked = true;
        }

    }
}