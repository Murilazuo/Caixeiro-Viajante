# Caixeiro-Viajante
 Lucas Eilert, Murilo Bezerra
## City
Armazena a posição das cidades

## StreetPath
Controla e modifica o caminho.
Possui uma Lista de inteiros que armazena o index das cidades na lista de cidades de Manager

### struct DividedStreerPath 
estrutura com duas listas de inteiros, uma para cada metade do caminho. 
DividedStreerPath(List<int> path) contrução recebe uma Lista de inteiro e divide na metate e adiciona cada metade a uma lista de inteiros. 
A propriedade MergedList retorna uma List<int> com as duas partes do caminho fundidas.
O método Crossover recebe uma DividedStreerPath parent2, adiciona a segunda metade do parent2 a si, checa se algum número está repetido na primeira parte e subistitui para index validos.

### Métodos
void CalculateTotalDistace() calcula a distance total de caminho.
void Initialize() gera um caminho aleatório entre as cidades.
void Crossover(List<int> parent1, List<int> parent2)  
void Mutate() Troca a posição de duas cidades aleatórias

## Manager
Instancia as cidade e estradas e controla a população e as gerações.

### Métodos
GenerateCity() Destroi as cidade e intancia um numero de cidade de acordo com a UI e em posições aleatórias.
GeneratePath() Destroi os caminhos existentes e Instancia e inicializa um numero de caminhos equivalentes a UI.
SelectPaths() Organiza os caminhos do caminho mais curto ao maior
NextGeneration() Divide os caminho em 4 quartos, o último quarto (caminhos mais longos) são inicializados novamente, o primeiro e    mantido (melhor caminho) é mantido, o restante dos caminhos sofre crossover entre si, contando com o primeiro. possui uma change de sobrer uma mutação chamando Mutate do caminho.
AutoPlay() Chama NextGeneration a cada frame
 
