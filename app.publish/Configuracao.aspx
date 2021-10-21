<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Configuracao.aspx.cs" Inherits="EstaparCRUD.Configuracao" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(window).on('load', function (event) {
            $('#preloader1').delay(0).fadeOut('slow', function () {
            });
        });
        $(function () {

            $(".btn-preloader").on('click', function (event) {
                $('#preloader1').delay(0).fadeIn('fast', function () {
                });
                $("#genCortina").fadeIn('fast');
            });
        });

    </script>
    <div class="card card-body">
        <div class="form-group row"></div>
        <div class="card card-default">
            <div class="card-header">
                <div class="row">
                    <div class="col-12 text-center">
                        CONFIGURAÇÕES DO SISTEMA
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <span class="col-2">MARCAS CARRO:</span>
                    <div class="col-3">
                      <asp:DropDownList runat="server" CssClass="form-control" ID="ddlMarcas" OnInit="ddlMarcas_Init" OnSelectedIndexChanged="ddlMarcas_SelectedIndexChanged" AutoPostBack="true">
                      </asp:DropDownList>
                    </div>
                    <div class="col text-center">
                        <asp:LinkButton ID="lnkRemoverMarca" runat="server" OnClick="lnkRemoverMarca_Click" CssClass="btn btn-danger btn-preloader">
                            -
                        </asp:LinkButton>
                    </div>
                    <span class="col-2">MODELOS CARRO:</span>
                    <div class="col-3">
                      <asp:DropDownList runat="server" CssClass="form-control" ID="ddlModelos">
                      </asp:DropDownList>
                    </div>
                    <div class="col text-center">
                        <asp:LinkButton ID="lnkRemoverModelo" runat="server" OnClick="lnkRemoverModelo_Click" CssClass="btn btn-danger btn-preloader">
                            -
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="form-group row">
                    <span class="col-2">NOVA MARCA:</span>
                    <div class="col-3">
                        <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        <small class="busca">Esse campo é obrigatório!</small>
                   </div>
                    <div class="col text-center">
                        <asp:LinkButton ID="btnCadastrarMarca" runat="server" OnClick="btnCadastrarMarca_Click" CssClass="btn btn-success btn-preloader">
                            +
                        </asp:LinkButton>
                    </div>
                    <span class="col-2">NOVO MODELO:</span>
                    <div class="col-3">
                        <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        <small class="busca">Esse campo é obrigatório!</small>
                   </div>
                    <div class="col text-center">
                        <asp:LinkButton ID="btnCadastrarModelo" runat="server" OnClick="btnCadastrarModelo_Click" CssClass="btn btn-success btn-preloader">
                            +
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divCortina" runat="server" visible="false"></div>
</asp:Content>
