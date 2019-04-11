using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apMatriz
{
    public partial class frmMatriz : Form
    {
        ListaLigadaCruzada matrizEsparsa;
        String nomeArquivo;
        public frmMatriz()
        {
            InitializeComponent();
        }
        private void btnOperacoes_Click(object sender, EventArgs e)
        {
            frmOperacoes frmOperacoes = new frmOperacoes();
            frmOperacoes.Show();
            this.Hide();
        }

        private void frmMatriz_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnLerArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlgAbrir.ShowDialog() == DialogResult.OK) // usuário selecionou o arquivo
                {
                    nomeArquivo = dlgAbrir.FileName;
                    StreamReader arquivo = new StreamReader(nomeArquivo);

                    string linhaArquivo = arquivo.ReadLine();

                    string[] coordenadas = linhaArquivo.Split(' ');  // tira os espaços que separam a qtde de linhas e colunas
                                                                     // e coloca num vetor os dois valores

                    int linhas = Convert.ToInt32(coordenadas[0]);
                    int colunas = Convert.ToInt32(coordenadas[1]);

                    matrizEsparsa = new ListaLigadaCruzada(linhas, colunas);

                    while ((linhaArquivo = arquivo.ReadLine()) != null)
                    {
                        string[] celula = linhaArquivo.Split(','); // dados separados por virgula no arquivo

                        double valor = Convert.ToDouble(celula[0]);
                        int linha = Convert.ToInt32(celula[1]);
                        int coluna = Convert.ToInt32(celula[2]);

                        matrizEsparsa.Inserir(valor, linha, coluna);
                    }
                    arquivo.Close(); // inseriu tudo na matriz



                    matrizEsparsa.ExibirDataGridView(dgvMatriz);

                    // seta os NumericUpDown com o número máximo de linhas/colunas
                    // (-1 porque o índice começa no 0)
                    linhaInserir.Maximum = linhas - 1;
                    colunaInserir.Maximum = colunas - 1;

                    linhaExcluir.Maximum = linhas - 1;
                    colunaExcluir.Maximum = colunas - 1;

                    linhaBuscar.Maximum = linhas - 1;
                    colunaBuscar.Maximum = colunas - 1;

                    colunaSoma.Maximum = colunas - 1;

                    // aumenta o tamanho das celulas
                    AumentarCelulas();

                    // habilita os botões das funções (inserir, excluir, buscar, limpar, somar na coluna)
                    btnBuscar.Enabled = true;
                    btnInserir.Enabled = true;
                    btnLimpar.Enabled = true;
                    btnExcluir.Enabled = true;
                    btnSomarColuna.Enabled = true;
                    btnSalvarArquivo.Enabled = true;
                }
            }
            catch(Exception erro)
            {
                MessageBox.Show("As linhas do arquivo devem estar no formato correto para a criação da matriz:\n" +
                                "[Linha] [Coluna]                 ->  ex: 5 6\n" +
                                "[Valor], [Linha], [Coluna]  ->  ex: 23, 4, 5", "Erro na criação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }


        private void btnCriar_Click(object sender, EventArgs e)
        {
            // os campos do forms já não permitem esses valores, mas a verificação é feita novamente
            if (numLinhas.Value > 0 && numColunas.Value > 0)
            {
                // seta as linhas e colunas e cria uma lista cruzada com os valores, exibindo-a no gridview
                int linhas = Convert.ToInt32(numLinhas.Value);
                int colunas = Convert.ToInt32(numColunas.Value);
                matrizEsparsa = new ListaLigadaCruzada(linhas, colunas);
                matrizEsparsa.ExibirDataGridView(dgvMatriz);

                // seta os NumericUpDown com o número máximo de linhas/colunas
                // (-1 porque o índice começa no 0)
                linhaInserir.Maximum = linhas - 1;   
                colunaInserir.Maximum = colunas - 1;

                linhaExcluir.Maximum = linhas - 1;
                colunaExcluir.Maximum = colunas - 1;

                linhaBuscar.Maximum = linhas - 1;
                colunaBuscar.Maximum = colunas - 1;

                colunaSoma.Maximum = colunas - 1;

                // aumenta o tamanho das celulas
                AumentarCelulas();

                // habilita os botões das funções (inserir, excluir, buscar, limpar, somar na coluna)
                btnBuscar.Enabled = true;
                btnInserir.Enabled = true;
                btnLimpar.Enabled = true;
                btnExcluir.Enabled = true;
                btnSomarColuna.Enabled = true;

                if (btnSalvarArquivo.Enabled == true)
                    btnSalvarArquivo.Enabled = false;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                // desaloca a matriz e exibe (nada) no gridview
                matrizEsparsa.ApagarMatriz();
                matrizEsparsa.ExibirDataGridView(dgvMatriz);

                // desabilita os botões das funções (inserir, excluir, buscar, limpar, somar na coluna)
                btnBuscar.Enabled = false;
                btnInserir.Enabled = false;
                btnLimpar.Enabled = false;
                btnExcluir.Enabled = false;
                btnSomarColuna.Enabled = false;
                btnSalvarArquivo.Enabled = false;

                // seta tudo ao início para não ocorrer algum possível erro no futuro
                linhaInserir.Value = 0;
                colunaInserir.Value = 0;
                linhaExcluir.Value = 0;
                colunaExcluir.Value = 0;
                linhaBuscar.Value = 0;
                colunaBuscar.Value = 0;
                colunaSoma.Value = 0;
            }
            // não chegará aqui pois esse botão fica desabilitado quando não há matriz
            catch (Exception erro) 
            {
                MessageBox.Show("Matriz inexistente", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            // verifica campo vazio
            if (txtNumeroInserir.Text == "")
            {
                MessageBox.Show("Digite um valor para ser inserido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                matrizEsparsa.Inserir(int.Parse(txtNumeroInserir.Text), Convert.ToInt32(linhaInserir.Value), Convert.ToInt32(colunaInserir.Value));
                matrizEsparsa.ExibirDataGridView(dgvMatriz);
                AumentarCelulas();
            }
            catch(Exception erro)  // único erro que pode ocorrer é esse pois o resto foi tratado no forms
            {
                MessageBox.Show("Certifique-se de que o valor digitado é um número", "Erro ao inserir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // não terá erro pois o numericUpDown já faz a verificação
            matrizEsparsa.RemoverEm(Convert.ToInt32(linhaExcluir.Value), Convert.ToInt32(colunaExcluir.Value));
            matrizEsparsa.ExibirDataGridView(dgvMatriz);
            AumentarCelulas();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            lblResultado.Text = "Resultado: " + matrizEsparsa.ValorDe(Convert.ToInt32(linhaBuscar.Value), Convert.ToInt32(colunaBuscar.Value)) + "                ";
        }

        // click do botão somar na coluna
        private void button1_Click(object sender, EventArgs e)
        {
            // verifica campo vazio
            if (txtValorSoma.Text == "")
            {
                MessageBox.Show("Digite um valor para ser somado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // tenta converter a string para inteiro, lança exception se não conseguir
            double numero;
            if (double.TryParse(txtValorSoma.Text, out numero) == false)
            {
                MessageBox.Show("Certifique-se de que o valor digitado é um número", "Erro ao somar nas colunas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // tenta somar na coluna. caso não exista a coluna (praticamente impossível pois os numberUpDown 
            // não permitem), lança exception

            try
            {
                matrizEsparsa.SomarNaColuna(numero, Convert.ToInt32(colunaSoma.Value));
                matrizEsparsa.ExibirDataGridView(dgvMatriz);
                AumentarCelulas();
            }
            catch(Exception erro)
            {
                MessageBox.Show(null, "Erro ao somar nesta coluna: certifique-se de que ela existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMatriz_Load(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, 65) + "matrizestexto";
            dlgAbrir.InitialDirectory = path;
        }

        private void AumentarCelulas()
        {
            for (int i = 0; i < dgvMatriz.Rows.Count; i++)
                dgvMatriz.Rows[i].Height = 30;
        }

        private void btnSalvarArquivo_Click(object sender, EventArgs e)
        {
            matrizEsparsa.SalvarEmArquivo(new StreamWriter(nomeArquivo));
        }
    }
}
