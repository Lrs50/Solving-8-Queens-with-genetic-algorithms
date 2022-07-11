using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Text genText;
    public Text fitnessText;
    public Text rankingText;

    private GameObject[] queens;
    private static int[] validPos;
    DNA[] population;
    DNA[] children;
    int populationSize = 10;

    void Start()
    {
        queens = new GameObject[8];
        population = new DNA[populationSize];
        validPos = new int[] {0,1,2,3,4,5,6,7};

        for(int i=0;i<populationSize;i++){
            population[i]=new DNA();
        }

        StartCoroutine(Run());

    }

    public Vector2 MapPosition(Vector2 pos){
        Vector3 newPos = new Vector3(-7.5f,0.4f,0);

        newPos.x += 0.5f*(pos.x + pos.y);
        newPos.y += 0.25f*(-pos.x + pos.y);

        return newPos;
    }
  
    public static List<int> Shuffle(){
        List<int> temp = new List<int>();
        List<int> copy = new List<int>(validPos);

        int n = copy.Count;

        while(n>0){
            int index = Random.Range(0,n);
            temp.Add(copy[index]);
            copy.Remove(copy[index]);
            n = copy.Count;
        }

        return temp;
    }

    DNA GetMostFit(){

        int index = 0;
        int bestFitness = population[index].GetFitness();

        for(int i=1;i<populationSize;i++){
            int fitness = population[i].GetFitness();
            if(fitness<=bestFitness){
                index=i;
                bestFitness=fitness;
            }
        }

        return population[index];

    }

     List<DNA> GenerateRoulette(){
        List<DNA> roulette = new List<DNA>();

        foreach(DNA individual in population){
            int count = 5 - individual.GetFitness();
            if(count <=0){
                count = 1;
            }
            for(int i=0;i<count;i++){
                roulette.Add(individual);
            }
        }

        return roulette;
    }

    DNA PickInRoulette(List<DNA> roulette){
        DNA luckyOne = roulette[Random.Range(0,roulette.Count)];

        return luckyOne;
    }

    float AvgFitness(){
        float val = 0;

        foreach(DNA individual in population){
            val += individual.GetFitness();
        }

        return val/populationSize;
    }

    void UpdateData(int generation,int bestCurrentFitness){
        fitnessText.text = $"Collisions: {bestCurrentFitness}";
        genText.text =$"Generation: {generation}";
        string currentRank = $"    Population          Fitness Rank Avg({System.Math.Round(AvgFitness(),1)})";

        GFG gg = new GFG();
        List<DNA> population2 = new List<DNA>(population);
        population2.Sort(gg);

        for(int i=0;i<populationSize;i++){
            if(i>=10){
                break;
            }
            currentRank+= "\r\n"+population2[i].Show();
        }

        rankingText.text = currentRank;

    }

    IEnumerator Run(){

        yield return new WaitForSeconds(2);

        DNA mostFit = GetMostFit();
        int bestCurrentFitness = mostFit.GetFitness();
        List<int> pos = mostFit.genes;
        int generation = 0;

        for(int i=0;i<8;i++){
             queens[i] = Instantiate(player,MapPosition(new Vector3(i,pos[i])),Quaternion.identity);
             yield return new WaitForSeconds(0.1f);
        }

        UpdateData(generation,bestCurrentFitness);

        yield return new WaitForSeconds(1);
        while(bestCurrentFitness>0){

            generation++;
            List<DNA> roulette = GenerateRoulette();
            float choice = 0;
            DNA[] newPopulation = new DNA[populationSize];

            for(int i=0;i<populationSize;i++){
                choice = Random.Range(0,1f);
                if(choice<0.7f){
                    newPopulation[i] = new DNA(PickInRoulette(roulette),PickInRoulette(roulette));
                }else{
                    newPopulation[i] = new DNA(PickInRoulette(roulette));
                }
            }
            population = newPopulation;

            mostFit = GetMostFit();
            bestCurrentFitness = mostFit.GetFitness();
            pos = mostFit.genes;

            for(int i=0;i<8;i++){
                queens[i].GetComponent<Transform>().position = MapPosition(new Vector3(i,pos[i]));
                yield return new WaitForSeconds(0.1f);
            }

            UpdateData(generation,bestCurrentFitness);
            yield return new WaitForSeconds(0.2f);
        }
            
    }



}
