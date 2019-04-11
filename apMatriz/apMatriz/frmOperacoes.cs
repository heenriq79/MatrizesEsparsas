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
    public partial class frmOperacoes : Form
    {

        ListaLigadaCruzada matrizEsparsa1;
        ListaLigadaCruzada matrizEsparsa2;
        ListaLigadaCruzada matrizEsparsaResultado;
        public frmOperacoes()
        {
            InitializeComponent();
        }

        private void frmOperacoes_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmMatriz frmMatriz = new frmMatriz();
            this.Hide();
            frmMatriz.Show();
        }

        private void btnCriarMatrizEsparsa1_Click(object sender, EventArgs e)
        {
            // os campos do forms já não permitem esses valores, mas a verificação é feita novamente
            if (numLinhasMatriz1.Value > 0 && numColunasMatriz1.Value > 0)
            {
                // seta as linhas e colunas e cria uma lista cruzada com os valores, exibindo-a no gridview
                int linhas = Convert.ToInt32(numLinhasMatriz1.Value);
                int colunas = Convert.ToInt32(numColunasMatriz1.Value);

                matrizEsparsa1 = new ListaLigadaCruzada(linhas, colunas);
                matrizEsparsa1.ExibirDataGridView(dgvMatriz1);

                // seta os NumericUpDown com o número máximo de linhas/colunas
                // (-1 porque o índice começa no 0)
                linhaInserir1.Maximum = linhas - 1;
                colunaInserir1.Maximum = colunas - 1;

                btnInserir1.Enabled = true;
                btnExcluir1.Enabled = true;

                if (matrizEsparsa2 == null)
                    return;
                if (matrizEsparsa2.EstaVazia)
                    return;

                btnSomar.Enabled = true;
                btnMultiplicar.Enabled = true;
            }
        }

        private void btnInserir1_Click(object sender, EventArgs e)
        {
            // verifica campo vazio
            if (txtNumeroInserir1.Text == "")
            {
                MessageBox.Show("Digite um valor para ser inserido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                matrizEsparsa1.Inserir(int.Parse(txtNumeroInserir1.Text), Convert.ToInt32(linhaInserir1.Value), Convert.ToInt32(colunaInserir1.Value));
                matrizEsparsa1.ExibirDataGridView(dgvMatriz1);
            }
            catch (Exception erro)  // único erro que pode ocorrer é esse pois o resto foi tratado no forms
            {
                MessageBox.Show("Certifique-se de que o valor digitado é um número", "Erro ao inserir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir1_Click(object sender, EventArgs e)
        {
            // não terá erro pois o numericUpDown já faz a verificação
            matrizEsparsa1.RemoverEm(Convert.ToInt32(linhaInserir1.Value), Convert.ToInt32(colunaInserir1.Value));
            matrizEsparsa1.ExibirDataGridView(dgvMatriz1);
        }

        private void btnCriarMatrizEsparsa2_Click(object sender, EventArgs e)
        {
            // os campos do forms já não permitem esses valores, mas a verificação é feita novamente
            if (numLinhasMatriz2.Value > 0 && numColunasMatriz2.Value > 0)
            {
                // seta as linhas e colunas e cria uma lista cruzada com os valores, exibindo-a no gridview
                int linhas = Convert.ToInt32(numLinhasMatriz2.Value);
                int colunas = Convert.ToInt32(numColunasMatriz2.Value);

                matrizEsparsa2 = new ListaLigadaCruzada(linhas, colunas);
                matrizEsparsa2.ExibirDataGridView(dgvMatriz2);

                // seta os NumericUpDown com o número máximo de linhas/colunas
                // (-1 porque o índice começa no 0)
                linhaInserir2.Maximum = linhas - 1;
                colunaInserir2.Maximum = colunas - 1;

                btnInserir2.Enabled = true;
                btnExcluir2.Enabled = true;

                if (matrizEsparsa2 == null)
                    return;
                if (matrizEsparsa2.EstaVazia)
                    return;

                btnSomar.Enabled = true;
                btnMultiplicar.Enabled = true;
            }
        }

        private void btnInserir2_Click(object sender, EventArgs e)
        {
            // verifica campo vazio
            if (txtNumeroInserir2.Text == "")
            {
                MessageBox.Show("Digite um valor para ser inserido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                matrizEsparsa2.Inserir(int.Parse(txtNumeroInserir2.Text), Convert.ToInt32(linhaInserir2.Value), Convert.ToInt32(colunaInserir2.Value));
                matrizEsparsa2.ExibirDataGridView(dgvMatriz2);
            }
            catch (Exception erro)  // único erro que pode ocorrer é esse pois o resto foi tratado no forms
            {
                MessageBox.Show("Certifique-se de que o valor digitado é um número", "Erro ao inserir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir2_Click(object sender, EventArgs e)
        {
            // não terá erro pois o numericUpDown já faz a verificação
            matrizEsparsa2.RemoverEm(Convert.ToInt32(linhaInserir2.Value), Convert.ToInt32(colunaInserir2.Value));
            matrizEsparsa2.ExibirDataGridView(dgvMatriz2);
        }

        private void btnSomar_Click(object sender, EventArgs e)
        {
            try
            {
                matrizEsparsaResultado = matrizEsparsa1.SomarMatrizes(matrizEsparsa2);
                matrizEsparsaResultado.ExibirDataGridView(dgvMatrizResultado);

                btnInserir3.Enabled = true;
                btnExcluir3.Enabled = true;
                btnListarCelulas.Enabled = true;

                // seta os NumericUpDown com o número máximo de linhas/colunas
                // (-1 porque o índice começa no 0)
                linhaInserirResultado.Maximum = matrizEsparsaResultado.Linhas - 1;
                colunaInserirResultado.Maximum = matrizEsparsaResultado.Colunas - 1;
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnMultiplicar_Click(object sender, EventArgs e)
        {
            try
            {
                matrizEsparsaResultado = matrizEsparsa1.MultiplicarMatrizes(matrizEsparsa2);
                matrizEsparsaResultado.ExibirDataGridView(dgvMatrizResultado);
                btnInserir3.Enabled = true;
                btnExcluir3.Enabled = true;
                btnListarCelulas.Enabled = true;

                // seta os NumericUpDown com o número máximo de linhas/colunas
                // (-1 porque o índice começa no 0)
                linhaInserirResultado.Maximum = matrizEsparsaResultado.Linhas - 1;
                colunaInserirResultado.Maximum = matrizEsparsaResultado.Colunas - 1;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // lista, utilizando o método toList da lista cruzada, os dados da lista/matriz
        private void btnListarCelulas_Click(object sender, EventArgs e)
        {
            lsbCelulas.Items.Clear();
            ListBox lsbMatrizResultado = matrizEsparsaResultado.ToList();

            int qtd = lsbMatrizResultado.Items.Count;

            for (int i = 0; i < qtd; i++)
                 lsbCelulas.Items.Add(lsbMatrizResultado.Items[i]);
        }

        private void btnLerArquivo1_Click(object sender, EventArgs e)
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK) // usuário selecionou o arquivo
            {
                StreamReader arquivo = new StreamReader(dlgAbrir.FileName);

                string linhaArquivo = arquivo.ReadLine();

                string[] coordenadas = linhaArquivo.Split(' ');  // tira os espaços que separam a qtde de linhas e colunas
                                                                 // e coloca num vetor os dois valores

                int linhas = Convert.ToInt32(coordenadas[0]);
                int colunas = Convert.ToInt32(coordenadas[1]);

                matrizEsparsa1 = new ListaLigadaCruzada(linhas, colunas);

                while ((linhaArquivo = arquivo.ReadLine()) != null)
                {
                    string[] celula = linhaArquivo.Split(','); // dados separados por virgula no arquivo

                    double valor = Convert.ToDouble(celula[0]);
                    int linha = Convert.ToInt32(celula[1]);
                    int coluna = Convert.ToInt32(celula[2]);

                    matrizEsparsa1.Inserir(valor, linha, coluna);
                }
                arquivo.Close(); // inseriu tudo na matriz



                matrizEsparsa1.ExibirDataGridView(dgvMatriz1);

                // seta os NumericUpDown com o número máximo de linhas/colunas
                // (-1 porque o índice começa no 0)
                linhaInserir1.Maximum = linhas - 1;
                colunaInserir1.Maximum = colunas - 1;


                // habilita os botões das funções (inserir e excluir)
                btnInserir1.Enabled = true;
                btnExcluir1.Enabled = true;

                // habilita somar e multiplicar caso as duas matrizes sejam existentes
                if (matrizEsparsa2 == null)
                    return;
                if (matrizEsparsa2.EstaVazia)
                    return;

                btnSomar.Enabled = true;
                btnMultiplicar.Enabled = true;
            }
        }


        private void btnLerArquivo2_Click(object sender, EventArgs e)
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK) // usuário selecionou o arquivo
            {
                StreamReader arquivo = new StreamReader(dlgAbrir.FileName);

                string linhaArquivo = arquivo.ReadLine();

                string[] coordenadas = linhaArquivo.Split(' ');  // tira os espaços que separam a qtde de linhas e colunas
                                                                 // e coloca num vetor os dois valores

                int linhas = Convert.ToInt32(coordenadas[0]);
                int colunas = Convert.ToInt32(coordenadas[1]);

                matrizEsparsa2 = new ListaLigadaCruzada(linhas, colunas);

                while ((linhaArquivo = arquivo.ReadLine()) != null)
                {
                    string[] celula = linhaArquivo.Split(','); // dados separados por virgula no arquivo

                    double valor = Convert.ToDouble(celula[0]);
                    int linha = Convert.ToInt32(celula[1]);
                    int coluna = Convert.ToInt32(celula[2]);

                    matrizEsparsa2.Inserir(valor, linha, coluna);
                }
                arquivo.Close(); // inseriu tudo na matriz



                matrizEsparsa2.ExibirDataGridView(dgvMatriz2);

                // seta os NumericUpDown com o número máximo de linhas/colunas
                // (-1 porque o índice começa no 0)
                linhaInserir2.Maximum = linhas - 1;
                colunaInserir2.Maximum = colunas - 1;


                // habilita os botões das funções (inserir e excluir)
                btnInserir2.Enabled = true;
                btnExcluir2.Enabled = true;

                // habilita somar e multiplicar caso as duas matrizes sejam existentes
                if (matrizEsparsa2 == null)
                    return;
                if (matrizEsparsa2.EstaVazia)
                    return;

                btnSomar.Enabled = true;
                btnMultiplicar.Enabled = true;
            }
        }

        private void frmOperacoes_Load(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, 65) + "matrizestexto";
            dlgAbrir.InitialDirectory = path;
        }

        private void btnInserir3_Click(object sender, EventArgs e)
        {
            // verifica campo vazio
            if (txtNumeroInserirResultado.Text == "")
            {
                MessageBox.Show("Digite um valor para ser inserido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                matrizEsparsaResultado.Inserir(int.Parse(txtNumeroInserirResultado.Text), Convert.ToInt32(linhaInserirResultado.Value), Convert.ToInt32(colunaInserirResultado.Value));
                matrizEsparsaResultado.ExibirDataGridView(dgvMatrizResultado);
            }
            catch (Exception erro)  // único erro que pode ocorrer é esse pois o resto foi tratado no forms
            {
                MessageBox.Show("Certifique-se de que o valor digitado é um número", "Erro ao inserir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir3_Click(object sender, EventArgs e)
        {
            // não terá erro pois o numericUpDown já faz a verificação
            matrizEsparsaResultado.RemoverEm(Convert.ToInt32(linhaInserirResultado.Value), Convert.ToInt32(colunaInserirResultado.Value));
            matrizEsparsaResultado.ExibirDataGridView(dgvMatrizResultado);
        }
    }
}
