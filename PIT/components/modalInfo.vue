<template>
    <div>
        <v-card class="pa-4">
            <div v-if="payment == false">


                <v-row class="ma-0 pb-4">
                    <v-col cols="12">
                        <div style="text-align:center; color: #6A44A5; font-weight: 700; font-size: 22px;">
                            Endereço
                        </div>
                    </v-col>
                    <v-col cols="8">
                        Rua
                        <v-text-field v-model="user.street" hide-details="auto" outlined>

                        </v-text-field>
                    </v-col>
                    <v-col>
                        Número
                        <v-text-field v-model="user.number" type="number" hide-details="auto" outlined>
                        </v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <div class="mt-4">
                            Complemento
                            <v-text-field v-model="user.complement" hide-details="auto" outlined>
                            </v-text-field>
                        </div>
                    </v-col>
                    <v-col>
                        <div class="d-flex" style="justify-content: flex-end;">
                            <v-btn @click="!payment ? payment = true : payment = false" icon>
                                <v-icon color="primary">
                                    mdi-arrow-right
                                </v-icon>
                            </v-btn>
                        </div>
                    </v-col>
                </v-row>
            </div> <v-form v-else>
                <v-row class="ma-0 pb-4">
                    <v-col>
                        <div style="text-align:center; color: #6A44A5; font-weight: 700; font-size: 22px;">
                            Pagamento
                        </div>
                    </v-col>
                    <v-col cols="12">
                        <div>
                            Número do cartão
                            <v-text-field v-model="user.cardNumber" v-mask="'####-####-####-####'" hide-details="auto"
                                outlined>
                            </v-text-field>
                        </div>
                    </v-col>
                    <v-col cols="6">
                        <div class="mt-4">
                            Código de verificação
                            <v-text-field v-model="user.code" v-mask="'###'" hide-details="auto" outlined>
                            </v-text-field>
                        </div>
                    </v-col><v-col cols="6">
                        <div class="mt-4">
                            Validade
                            <v-text-field v-model="user.validity" v-mask="'##/##'" hide-details="auto" outlined>
                            </v-text-field>
                        </div>
                    </v-col>
                    <v-col cols="12">
                        <div class="d-flex" style="justify-content: flex-end;">
                            Valor do café: {{ product.price }}
                        </div>
                    </v-col>
                    <v-col>
                        <div class="d-flex">
                            <v-btn @click="!payment ? payment = true : payment = false" icon>
                                <v-icon color="primary">
                                    mdi-arrow-left
                                </v-icon>
                            </v-btn>
                        </div>
                    </v-col>
                </v-row>

                <div class="d-flex mt-2 mb-4" style="justify-content: center;">
                    <v-btn @click="editUser" :loading="loading" class="primary">Finalizar compra</v-btn>
                </div>
            </v-form>
        </v-card>
    </div>
</template>
<script>
export default {
    name: "modalInfo",
    data: () => ({
        userId: null,
        user: {},
        payment: false,
        loading: false
    }),

    props: {
        prop_product: Object
    },

    async created() {
        this.product = this.prop_product
        this.userId = JSON.parse(localStorage.getItem("user")).id;
        await this.getUser()
    },

    methods: {
        async getUser() {
            await this.$axios
                .$get(`user/${this.userId}`)
                .then((res) => {
                    this.user = res.content
                })
                .catch((res) => {
                });
        },

        editUser() {
            const model = {
                UserId: this.userId,
                Complement: this.user.complement,
                Number: this.user.number,
                CardNumber: this.user.cardNumber,
                Code: this.user.code,
                Validity: this.user.validity,
                Street: this.user.street
            }
            this.loading = true
            this.$axios
                .$post("user/edit", model)
                .then((res) => {
                    this.$toast.success("Compra realizada com sucesso!")
                    this.$emit('close')
                    this.loading = false
                })
                .catch((res) => {
                    this.loading = false
                });
        }
    },
};
</script>