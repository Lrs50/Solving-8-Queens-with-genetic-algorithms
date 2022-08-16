# Computação Evolutiva aplicada ao Problema das 8 Rainhas

  

### Projeto CIN - UFPE 2022

### Miniprojeto 1 da disciplina de Computação Bioinspirada

### Professor: Paulo Salgado Gomes de Mattos Neto

### Equipe
* Lucas Reis (lrs5)
* Gabriel Ferreira (gfr)
* Lucca Limongi (laldf)

Simulador de um algoritmo evolutivo resolvendo o problema das 8 Rainhas [Link](https://lrs50.github.io/Solving-8-Queens-with-genetic-algorithms/WebGL%20Player/):

## Especificação Mini-projeto – 8 rainhas

- Representação (genótipo): números de 0 à quantidade de rainhas - 1
- Recombinação: “random-choice-between-parents” crossover
- Probabilidade de Recombinação: 70%
- Mutação: troca de genes
- Probabilidade de Mutação: 50%
- Seleção de pais: Roleta "equilibrada"
- Seleção de sobreviventes: Melhores filhos nos lugares dos pais
- Tamanho da população: 30
- Número de filhos gerados: 30
- Inicialização: aleatória
- Condição de término: Encontrar a solução, ou 10.000 avaliações de fitness
- Fitness: Quantidade de colisões total dividido por 2
