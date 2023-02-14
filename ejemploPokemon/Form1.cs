using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Datos;

namespace ejemploPokemon
{
    public partial class Form1 : Form
    {
        private List<Pokemon> ListaPokemon;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cargar();
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvPokemons.CurrentRow != null)
            {
                Pokemon Seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                CargarImagen(Seleccionado.UrlImagen);
            }
        }

        private void Cargar()
        {
            try
            {
                PokemonDatos Datos = new PokemonDatos();
                ListaPokemon = Datos.listar();
                dgvPokemons.DataSource = ListaPokemon;
                OcultarColumnas();
                CargarImagen(ListaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void OcultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
        }

        private void CargarImagen(string Imagen)
        {
            try
            {
                pbxPokemons.Load(Imagen);
            }
            catch (Exception ex)
            {

                pbxPokemons.Load("https://tse4.mm.bing.net/th?id=OIP.dxt2_gkvMt-3ZUVo8RF9SQHaHa&pid=Api&P=0");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmPokemonNuevo Nuevo = new frmPokemonNuevo();
            Nuevo.ShowDialog();
            Cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon Seleccionado;
            Seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            frmPokemonNuevo Modificar = new frmPokemonNuevo(Seleccionado);
            Modificar.Text = "Modificar Pokemon";
            Modificar.ShowDialog();
            Cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            Eliminar(true);
        }
        private void Eliminar(bool logico = false)
        {
            PokemonDatos datos = new PokemonDatos();
            Pokemon Seleccionado;
            try
            {
                DialogResult resultado = MessageBox.Show("¿Esta seguro que quiere eliminar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    Seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    if (logico)
                        datos.EliminarLogico(Seleccionado.Id);
                    else
                        datos.EliminarFisico(Seleccionado.Id);

                    Cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
        }

        private void txtbxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> ListaFiltrada;
            string filtro = txtbxFiltroRapido.Text;
            if (filtro.Length >= 3)
            {
                ListaFiltrada = ListaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                ListaFiltrada = ListaPokemon;
            }
            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = ListaFiltrada;
            OcultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if(opcion == "Número")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private bool ValidarFiltro()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el criterio para filtrar");
                return true;
            }

            if(string.IsNullOrEmpty(txtFiltro.Text))
            {
                MessageBox.Show("Debes cargar el filtro si usas el campo numerico");
                return true;
            }

            if(!(SoloNumeros(txtFiltro.Text)))
            {
                MessageBox.Show("Solo numeros en campo numerico por favor");
                return true;
            }

            return false;
        }

        private bool SoloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if(!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }

            return true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PokemonDatos datos = new PokemonDatos();
            try
            {
                if (ValidarFiltro())
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text.ToString();
                dgvPokemons.DataSource = datos.Filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
