import * as React from "react";
import { inject } from "mobx-react";
import { Form, Input, Button, Alert } from "antd";
import { Formik } from "formik";
import * as yup from "yup";

const { TextArea } = Input;

@inject("ReviewStore")
class ReplyForm extends React.PureComponent {
    render() {
        const {
            onSubmit
        } = this.props;

        return (
            <Formik
                initialValues={{
                    text: ""
                }}
                validationSchema={
                    yup.object().shape({
                        text: yup
                            .string()
                            .min(3, "please enter reply text")
                            .max(200)
                            .required()
                    })
                }
                onSubmit={async (values, formikBag) => {
                    formikBag.setStatus();
                    onSubmit(values, formikBag);
                }}
                render={({ values, errors, touched, handleChange, handleBlur, isSubmitting, status, handleSubmit }) => (
                    <Form onSubmit={handleSubmit}>
                        <Form.Item
                            help={touched.text && errors.text ? errors.text : ""}
                            validateStatus={touched.text && errors.text ? "error" : undefined}
                        >
                            <TextArea
                                name="text"
                                rows={4}
                                value={values.text}
                                onChange={handleChange}
                                onBlur={handleBlur}
                            />
                        </Form.Item>
                        <Form.Item>
                            <Button type="primary" size={"large"} htmlType="submit">
                                Submit
                            </Button>
                            {isSubmitting &&
                                <img style={{marginLeft: "10px"}} alt="loader" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                            }
                        </Form.Item>
                        {status &&
                            <Alert style={{ marginTop: "10px" }} type="error">{status}</Alert>
                        }
                    </Form>
                )}
            />
        )
    }
}

export default ReplyForm;