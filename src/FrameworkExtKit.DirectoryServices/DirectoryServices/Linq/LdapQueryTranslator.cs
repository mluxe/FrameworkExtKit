using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif


namespace FrameworkExtKit.Services.DirectoryServices.Linq {

    /*
     * 
     * This is the expression visitor that translate a .Net expression 
     * to raw Ldap query string.
     * 
     * There are some limitations here and it was not implemented in the best way
     * because the lack of knowledge behind the .Net LINQ technilogy.
     * 
     * We shall revisit the implementation later. 
     * 
     * Yufei Liu @ 2015-07-25
     */
    public class LdapQueryTranslator : ExpressionVisitor {

        private class OperandElement {
            public string Operand { get; set; }
            public int Count { get; set; }
        }
        protected IDictionary<string, string> propertyMap = new Dictionary<string, string>();

        private StringBuilder filterStringBuilder = new StringBuilder();

        private List<string> component_stack = new List<string>();
        Expression _expression;
        public LdapQueryTranslator(Expression expression, Type directoryAccountType) {

            MemberInfo[] memberInfos = directoryAccountType.GetMethods();

            foreach (var info in memberInfos) {
                var attr = (DirectoryPropertyAttribute)Attribute.GetCustomAttribute(info, typeof(DirectoryPropertyAttribute));
                if (attr != null) {
                    propertyMap.Add(info.Name, attr.SchemaAttributeName);
                }
            }

            PropertyInfo[] propertyInfos = directoryAccountType.GetProperties();

            foreach (var info in propertyInfos) {
                // var attrs = (DirectoryPropertyAttribute[])Attribute.GetCustomAttributes(info, typeof(DirectoryPropertyAttribute), false);
                var attr = (DirectoryPropertyAttribute)Attribute.GetCustomAttribute(info, typeof(DirectoryPropertyAttribute), false);
                if (attr != null) {
                    propertyMap.Add(info.Name, attr.SchemaAttributeName);
                }
            }

            this._expression = expression;
        }

        private string getLdapAttributeName(string memberName) {
            if (propertyMap.ContainsKey(memberName) == false) {
                return memberName;
            } else {
                return propertyMap[memberName];
            }
        }

        private string getLdapAttributeName(Expression exp) {
            if(exp.NodeType == ExpressionType.Constant) {
                return ((ConstantExpression)exp).Value.ToString();
            }

            if (exp.NodeType == ExpressionType.Convert) {
                return this.getLdapAttributeName(((UnaryExpression)exp).Operand);
            }

            if (exp is MemberExpression) {
                return this.getLdapAttributeName(((MemberExpression)exp).Member.Name);
            }

            throw new NotSupportedException($"unable to translate the expression type {exp.NodeType} to ldap attribute name, expression: " + exp.ToString());
        }

        private string getValue(Expression exp) {
            string value = "*";

            if (exp.NodeType == ExpressionType.Parameter) {
                value = this.getLdapAttributeName(exp);
            } else if (exp.NodeType == ExpressionType.Constant) {
                value = ((ConstantExpression)exp).Value.ToString();
            //} else if (exp.NodeType == ExpressionType.MemberAccess && ((MemberExpression)exp).Expression.NodeType == ExpressionType.Parameter) {
                //value = this.getLdapAttributeName((MemberExpression)exp).Member.Name);
            } else {
                if(exp.Type == typeof(int)) {
                    value = Expression.Lambda<Func<int>>(exp).Compile()().ToString();
                }else if(exp.Type == typeof(double)) {
                    value = Expression.Lambda<Func<double>>(exp).Compile()().ToString();
                }else if(exp.Type == typeof(double)) {
                    value = Expression.Lambda<Func<double>>(exp).Compile()().ToString();
                } else if (exp.Type == typeof(long)) {
                    value = Expression.Lambda<Func<long>>(exp).Compile()().ToString();
                } else if (exp.Type == typeof(short)) {
                    value = Expression.Lambda<Func<short>>(exp).Compile()().ToString();
                } else {
                    value = Expression.Lambda<Func<object>>(exp).Compile()().ToString();
                }
            }
            return value;
        }

        private IEnumerable<string> getValues(Expression exp) {
            var object_values = Expression.Lambda<Func<IEnumerable>>(exp).Compile()();
            var values = new List<string>();
            foreach (var v in object_values) {
                values.Add(v.ToString());
            }
            return values;
        }

        private StringBuilder constructFilterString(){
            Stack<OperandElement> operandStack = new Stack<OperandElement>();
            StringBuilder builder = new StringBuilder();

            foreach (var component in component_stack) {
                OperandElement currentOperand;
                if (component == "&" || component == "|") {
                    if (operandStack.Count == 0 || operandStack.Peek().Operand != component) {
                        currentOperand = new OperandElement { Operand = component, Count = 0 };
                        operandStack.Push(currentOperand);
                    } else {
                        currentOperand = operandStack.Peek();
                        currentOperand.Count++;
                    }

                    if (currentOperand.Count == 0) {
                        builder.Append("(" + currentOperand.Operand);
                    }
                }else if (component == "-&" || component == "-|") {
                    currentOperand = operandStack.Peek();

                    if (currentOperand.Count > 0) {
                        currentOperand.Count--;
                    } else {
                        builder.Append(")");
                        operandStack.Pop();
                    }
                } else {
                    //string filter = "(" + component + ")";
                    string filter = component;
                    builder.Append(filter);
                }   
            }

            return builder;
        }

        protected override Expression VisitUnary(UnaryExpression node) {
            component_stack.Add("(!");
            this.Visit(node.Operand);
            component_stack.Add(")");
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node) {
            string operand = String.Empty;
            string logical_operand = String.Empty;

            switch (node.NodeType) {
                case ExpressionType.AndAlso: logical_operand = "&"; break;
                case ExpressionType.OrElse: logical_operand = "|"; break;
                //case ExpressionType.Not: logical_operand = "!";break;
                default: logical_operand = String.Empty; break;
            }

            if(logical_operand != String.Empty) {
                component_stack.Add(logical_operand);
            } else {
                if(node.NodeType == ExpressionType.NotEqual) {
                    component_stack.Add("(!(");
                } else {
                    component_stack.Add("(");
                }   
            }

            Expression left = node.Left;
            Expression right = node.Right;

            if(right.NodeType == ExpressionType.MemberAccess && ((MemberExpression)right).Expression.NodeType == ExpressionType.Parameter) {
                //swap left and right expression
                Expression exchange = left;
                left = right;
                right = exchange;
            }

            this.Visit(left);
            switch (node.NodeType) {
                case ExpressionType.Equal: operand = "="; break;
                case ExpressionType.LessThan: operand = "<"; break;
                case ExpressionType.LessThanOrEqual: operand = "<="; break;
                case ExpressionType.GreaterThan: operand = ">"; break;
                case ExpressionType.GreaterThanOrEqual: operand = ">="; break;
                case ExpressionType.NotEqual: operand = "="; break;
                case ExpressionType.Not:
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    break;
                default:
                    throw new NotSupportedException(node.NodeType + " expression is not supported, expression: "+node.ToString());
                    //break;
            }
            component_stack.Add(operand);


            this.Visit(right);

            if (logical_operand != String.Empty) {
                component_stack.Add("-" + logical_operand);
            } else {
                if (node.NodeType == ExpressionType.NotEqual) {
                    component_stack.Add("))");
                } else {
                    component_stack.Add(")");
                }
            }
            
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node) {
            component_stack.Add(node.Value.ToString());
            return node;
        }

        protected override Expression VisitMember(MemberExpression node) {

            // String member = node.Member.Name;

            if (node.Expression.NodeType == ExpressionType.Parameter) {
                component_stack.Add(getLdapAttributeName(node));
            } 

            if (node.Expression.NodeType == ExpressionType.MemberAccess ||
                node.Expression.NodeType == ExpressionType.Constant) {
                string value = this.getValue(node);
                component_stack.Add(value);
            }

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node) {

            var methodName = node.Method.Name;
            var query = String.Empty;
            if(methodName == "ToString") {
                if (node.Object.NodeType == ExpressionType.MemberAccess){
                    if (((MemberExpression)node.Object).Expression.NodeType == ExpressionType.Parameter) {
                        query = this.getLdapAttributeName(node.Object);
                    } else {
                        query = this.getValue(node.Object);
                    }
                }
                
            } else if (/*node.Object.Type.IsValueType || */node.Object != null && node.Object.Type == typeof(string)) {
                IDictionary<string, string> supportedMethods = new Dictionary<string, string>{
                    { "StartsWith", "{0}*" },
                    { "EndsWith", "*{0}" },
                    { "Contains", "*{0}*" }
                };
                string value = String.Empty;

                if (!supportedMethods.ContainsKey(methodName)) {
                    throw new NotSupportedException($"Method call {methodName} on {node.ToString()} cannot be translated to ldap query syntax");
                }

                string valueFormat = supportedMethods[methodName];

                if (node.Arguments.Count > 0) {
                    value = this.getValue(node.Arguments[0]);
                }

                string ldap_attribute = getLdapAttributeName(node.Object);
                query = $"({ldap_attribute}={String.Format(valueFormat, value)})";
            } else if (node.Method.Name == "Contains") {
                //throw new NotSupportedException("Method call Array.Contains, List.Contains cannot be translated to ldap query syntax at the moment, this will be supported in the future.");


                Expression arg1 = node.Arguments[0];
                Expression arg2;
                IEnumerable<string> raw_values = new List<string>();

                if (node.Arguments.Count() > 1) {
                    arg2 = node.Arguments[1];
                } else {
                    arg2 = node.Object;
                }

                //if(node.Arguments.Count() > 1) {
                //     arg2 = node.Arguments[1];
                //}

                string ldap_attribute = String.Empty;

                if (arg1.NodeType == ExpressionType.NewArrayInit) {
                    MemberExpression memberExp = (MemberExpression)arg2;
                    Expression valueExp = arg1;
                    
                    raw_values = this.getValues(valueExp);
                    ldap_attribute = this.getLdapAttributeName(memberExp);
                }

                if(arg1.NodeType == ExpressionType.MemberAccess) {
                    MemberExpression callerExp = (MemberExpression)arg1;
                    if (callerExp.Expression.NodeType == ExpressionType.Constant) {

                        ldap_attribute = this.getLdapAttributeName(arg2);
                        if (node.Method.DeclaringType == typeof(Enumerable)) {
                            raw_values = this.getValues(callerExp);
                        }
                    }

                    if (callerExp.Expression.NodeType == ExpressionType.Parameter) {
                        ldap_attribute = this.getLdapAttributeName(callerExp);

                        if (arg2 is MemberExpression) {
                            if( ((MemberExpression)arg2).Expression.NodeType == ExpressionType.Parameter ) {
                                throw new NotSupportedException("lambda expression error: unableto build query from lambda paramter, a value must be supplied expression: " + node.ToString());
                            }
                        }

                        if(node.Object == null) {
                            string val = this.getValue(arg2);
                            raw_values = new string[] { val };
                        } else {
                            // value = this.getValue(arg2);
                            raw_values = this.getValues(arg2);
                        }

                    }
                }

                if(arg1.NodeType == ExpressionType.Call) {
                    raw_values = this.getValues(node.Object);
                    ldap_attribute = this.getLdapAttributeName(arg1);
                    
                }

                if(raw_values.Count() > 1) {
                    foreach (string value in raw_values) {
                        query += $"({ldap_attribute}={value})";
                    }
                    query = "(|" + query + ")";
                } else if (raw_values.Count() == 1) {
                    query = $"({ldap_attribute}={raw_values.FirstOrDefault()})";
                }

            }

            component_stack.Add(query);
            return node;
        }

        //public string QueryString { get { return "\n"+filterString.ToString()+"\n\n\n"+debugString.ToString(); } }
        public string FilterString { 
            get {
                if (filterStringBuilder.Length == 0) {
                    component_stack.Clear();
                    this.Visit(this._expression);
                    filterStringBuilder = this.constructFilterString();
                }
                return filterStringBuilder.ToString(); 
            } 
        }
    }
}
