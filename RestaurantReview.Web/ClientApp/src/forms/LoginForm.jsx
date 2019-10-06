import * as React from "react";
import { Form, Icon, Input, Button, Alert } from "antd";
import { Formik } from "formik";
import { Link } from "react-router-dom";
import * as yup from "yup";

class LoginForm extends React.Component {
    render() {
        const {
            onSubmit
        } = this.props;


        return (
            <Formik
                initialValues={{ username: "", password: "" }}
                validationSchema={yup.object().shape({
                    username: yup
                        .string()
                        .min(3, "email must be at least 3 characters")
                        .max(50)
                        .email("email must be a valid email")
                        .required(),
                    password: yup
                        .string()
                        .min(3, "password must be at least 3 characters")
                        .max(50)
                        .required()
                })}
                onSubmit={async (values, formikBag) => {
                    formikBag.setStatus();
                    await onSubmit(values, formikBag);
                }}
                render={({ values, handleChange, handleBlur, touched, errors, isSubmitting, status, handleSubmit }) => (
                    <Form style={{ width: 400, margin: "auto" }} onSubmit={handleSubmit}>
                        <Form.Item
                            help={touched.username && errors.username ? errors.username : ""}
                            validateStatus={touched.username && errors.username ? "error" : undefined}>
                            <Input
                                name="username"
                                prefix={<Icon type="user" style={{ color: "rgba(0,0,0,.25)" }} />}
                                placeholder="Email"
                                value={values.username}
                                onChange={handleChange}
                                onBlur={handleBlur}
                            />
                        </Form.Item>
                        <Form.Item
                            help={touched.password && errors.password ? errors.password : ""}
                            validateStatus={touched.password && errors.password ? "error" : undefined}>
                            <Input
                                name="password"
                                prefix={<Icon type="lock" style={{ color: "rgba(0,0,0,.25)" }} />}
                                type="password"
                                placeholder="Password"
                                value={values.password}
                                onChange={handleChange}
                                onBlur={handleBlur}
                            />
                        </Form.Item>
                        <Form.Item>
                            <Button
                                type="primary"
                                htmlType="submit"
                                disabled={isSubmitting}
                            >
                                Login
                            </Button>
                            {isSubmitting &&
                                <img style={{marginLeft: "10px"}} alt="loader" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                            }
                        </Form.Item>
                        Or <Link to="/register">register now!</Link>
                        {status &&
                            <Alert style={{marginTop: "10px"}} message={status} type="error" />
                        }
                    </Form>
                )}
            />
        );
    }
}

export default LoginForm;
