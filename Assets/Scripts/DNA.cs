using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {  
    public List<int> genes = new List<int>();
    public DNA(){
        genes = GameManager.Shuffle();
    }

    public DNA(DNA dad, DNA mum){
        List<int> choice = new List<int>();
        while(genes.Count<8){
            int i = genes.Count;
            if(Random.Range(0,2)==1){
                choice = dad.genes;
            }else{
                choice = mum.genes;
            }

            while(genes.Contains(choice[i])){
                i++;
                if(i>=8){
                    i=0;
                }
            }
            genes.Add(choice[i]);
        }
    }

    public DNA(DNA prior,float mutationRate=0.5f){

        genes = prior.genes;
        float mutationChance = Random.Range(0f,1f);
        if(mutationChance<=mutationRate){
            int pos1 = Random.Range(0,8);
            int pos2 = Random.Range(0,8);
            while(pos2==pos1){
                pos2 = Random.Range(0,8);
            }
            int val = genes[pos1];
            int val2 = genes[pos2];
            genes[pos1] = val2;
            genes[pos2] = val;
        }
    }

    public int GetFitness(){
        int fitness = 0;
        for(int n=0;n<genes.Count;n++){
            for(int i=0;i<genes.Count;i++){
                int dist = Mathf.Abs(n-i);
                if(dist==0){
                    continue;
                }
                if(genes[n]-dist==genes[i]||genes[n]+dist==genes[i]){
                    fitness++;
                }
                
            }
        }
        fitness/=2;
        return fitness;
    }

    public int CompareTo(DNA p){
        if(p.GetFitness()>GetFitness()){
            return -1;
        }
        return 1;
    }

    public string Show(){
        return $"[{genes[0]},{genes[1]},{genes[2]},{genes[3]},{genes[4]},{genes[5]},{genes[6]},{genes[7]}]	               {GetFitness()}";
    }
}

