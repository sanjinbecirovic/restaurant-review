import * as React from "react";
import { Form, Input, Button, Alert } from "antd";
import { Formik } from "formik";
import * as yup from "yup";

const { TextArea } = Input;

class RestaurantForm extends React.PureComponent {
    render() {
        const { onSubmit, getInitialValues } = this.props;
        return (
            <Formik
                initialValues={getInitialValues()}
                validationSchema={yup.object().shape({
                    name: yup
                        .string()
                        .min(5, "restaurant name must be at least 5 characters")
                        .max(50)
                        .required(),
                    address: yup
                        .string()
                        .min(5, "restaurant address must be at least 5 characters")
                        .max(50)
                        .required(),
                    description: yup
                        .string()
                        .min(5, "restaurant description must be at least 5 characters")
                        .max(200)
                        .required()

                })}
                onSubmit={async (values, formikBag) => {
                    formikBag.setStatus();
                    await onSubmit(values, formikBag);
                }}
                render={({ values, handleChange, handleBlur, touched, errors, isSubmitting, status, handleSubmit }) => (
                    <Form style={{ width: 400, margin: "auto" }} onSubmit={handleSubmit}>
                        <Form.Item
                            help={touched.name && errors.name ? errors.name : ""}
                            validateStatus={touched.name && errors.name ? "error" : undefined}
                        >
                            <Input
                                name="name"
                                placeholder="Name"
                                value={values.name}
                                onChange={handleChange}
                                onBlur={handleBlur}
                            />
                        </Form.Item>
                        <Form.Item
                            help={touched.address && errors.address ? errors.address : ""}
                            validateStatus={
                                touched.address && errors.address ? "error" : undefined
                            }
                        >
                            <Input
                                name="address"
                                placeholder="Address"
                                value={values.address}
                                onChange={handleChange}
                                onBlur={handleBlur}
                            />
                        </Form.Item>
                        <Form.Item
                            help={touched.description && errors.description ? errors.description : ""}
                            validateStatus={
                                touched.description && errors.description ? "error" : undefined
                            }>
                            <TextArea
                                name="description"
                                placeholder="Description"
                                rows={4}
                                value={values.description}
                                onChange={handleChange}
                                onBlur={handleBlur}
                            />
                        </Form.Item>
                        <Form.Item>
                            <Button
                                type="primary"
                                htmlType="submit"
                                className="login-form-button"
                            >
                                Submit
                            </Button>
                            {isSubmitting &&
                                <img style={{ marginLeft: "10px" }} alt="loader" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                            }
                        </Form.Item>
                        {status &&
                            <Alert style={{ marginTop: "10px" }} message={status} type="error" />
                        }
                    </Form>
                )}
            />
        );
    }
}

export default RestaurantForm;