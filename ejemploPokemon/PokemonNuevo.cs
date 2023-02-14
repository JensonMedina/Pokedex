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
using System.IO;
using System.Configuration;

namespace ejemploPokemon
{
    public partial class frmPokemonNuevo : Form
    {
        private Pokemon pokemon = null;
        private OpenFileDialog Archivo = null;
        public frmPokemonNuevo()
        {
            InitializeComponent();
        }
        public frmPokemonNuevo(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            PokemonDatos datos = new PokemonDatos();
            try
            {
                if (pokemon == null)
                    pokemon = new Pokemon();
                pokemon.Nombre = txtbxNombre.Text;
                pokemon.Descripcion = txtbxDescripcion.Text;
                pokemon.UrlImagen = txtbxUrlImagen.Text;
                pokemon.Tipo = (Elemento)cbxTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cbxDebilidad.SelectedItem;
                pokemon.Numero = int.Parse(txtbxNumero.Text);
                if (pokemon.Id != 0)
                {
                    datos.Modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    datos.Agregar(pokemon);
                    MessageBox.Show("Agregado exitosamente");
                }

                if(Archivo != null && !(txtbxUrlImagen.Text.ToUpper().Contains("HTTP")))
                    File.Copy(Archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + Archivo.SafeFileName);

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmPokemonNuevo_Load(object sender, EventArgs e)
        {
            ElementosDatos elementosDatos = new ElementosDatos();
            try
            {
                cbxTipo.DataSource = elementosDatos.listar();
                cbxTipo.ValueMember = "Id";
                cbxTipo.DisplayMember = "Descripcion";
                cbxDebilidad.DataSource = elementosDatos.listar();
                cbxDebilidad.ValueMember = "Id";
                cbxDebilidad.DisplayMember = "Descripcion";
                if (pokemon != null)
                {
                    txtbxNumero.Text = pokemon.Numero.ToString();
                    txtbxNombre.Text = pokemon.Nombre;
                    txtbxDescripcion.Text = pokemon.Descripcion;
                    txtbxUrlImagen.Text = pokemon.UrlImagen;
                    CargarImagen(pokemon.UrlImagen);
                    cbxTipo.SelectedValue = pokemon.Tipo.Id;
                    cbxDebilidad.SelectedValue = pokemon.Debilidad.Id;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtbxUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtbxUrlImagen.Text);
        }
        private void CargarImagen(string Imagen)
        {
            try
            {
                pbxPokemon.Load(Imagen);
            }
            catch (Exception ex)
            {

                pbxPokemon.Load("https://tse4.mm.bing.net/th?id=OIP.dxt2_gkvMt-3ZUVo8RF9SQHaHa&pid=Api&P=0");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            Archivo = new OpenFileDialog();
            Archivo.Filter = "jpg|*.jpg|png|*.png";
            if(Archivo.ShowDialog() == DialogResult.OK)
            {
                txtbxUrlImagen.Text = Archivo.FileName;
                CargarImagen(Archivo.FileName);
                //File.Copy(Archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + Archivo.SafeFileName);
            }
        }
    }
}
